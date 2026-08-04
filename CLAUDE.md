# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

StockManagement is a .NET 9 Blazor Web App (interactive WebAssembly render mode) for inventory, sales, and small-business finance/accounting. It uses a layered architecture over a SQL Server database that is driven **entirely through stored procedures** (no EF Core / ORM for domain data — Identity is the only EF exception, and even that uses custom stores).

## Build, Run, Test

All commands run from the repository root.

```bash
dotnet build StockManagement.sln
```

Run the app (host project — serves both the API and the Blazor client):

```bash
dotnet run --project StockManagement/StockManagement/StockManagement.csproj
```

Run the full stack (app + SQL Server 2022) via Docker Compose — app on `:8001`, SQL on `:8002`:

```bash
docker compose up -d
```

There is **no test project** in the solution. Do not assume a test runner exists.

The database is a SQL Server Database Project (`.sqlproj`). Schema and stored procedures live under `StockManagement.Database/` and are published/deployed as a DACPAC, not via migrations.

## Deployment

Production deploy is a manual PowerShell flow, not CI. `.github/workflows/` is empty. `PowerShell/DeployStockManagement.ps1` builds the Docker image, saves it to a tar, `scp`s it to a home server, and runs `docker compose up -d` over SSH. Bump the `"Version"` field in `appsettings.json` when releasing.

## Architecture

### Project layout & dependency direction

```
StockManagement.Client (Blazor WASM)  ──HTTP──▶  StockManagement (host: API + server render)
                                                        │
        Services.Interfaces ◀── Services ──▶ Repositories ◀── Repositories.Interfaces
                                                        │
                                                 Models (DTOs + Models, shared by all)
                                                        │
                                              StockManagement.Database (stored procs)
```

- **StockManagement.Models** — shared by every project. Contains two distinct model families:
  - `*Dto` (in `Dto/`) — database-shape objects passed between Repositories and Services only.
  - `*EditModel` / `*ResponseModel` — API/UI contracts. `EditModel` = inbound (create/update), `ResponseModel` = outbound (read). These are what controllers and the Blazor client exchange.
- **StockManagement.Repositories** — one class per aggregate, calls stored procedures via **Dapper**. Constructor-injected `IDbConnection` (a scoped `SqlConnection`).
- **StockManagement.Services** — thin orchestration layer; maps `EditModel ⇄ Dto` via **AutoMapper** and delegates to repositories.
- **StockManagement/StockManagement** — the ASP.NET Core host. Contains `ApiControllers/` (REST endpoints), Blazor server components, Identity, and cross-cutting services.
- **StockManagement.Client** — Blazor WebAssembly. Pages call typed **client data services** that wrap `HttpClient` against the host's `api/*` controllers.

### Request flow (follow this pattern for any new feature)

Blazor Page → **Client** `IXxxDataService` (HTTP) → `XxxController` (`api/[controller]`) → `IXxxService` (AutoMapper map) → `IXxxRepository` (Dapper) → `dbo.Xxx_*` stored procedure.

A single feature "vertical slice" therefore touches ~8 files across projects plus a stored procedure. Add DI registrations in **two** `Program.cs` files (see below).

### Two Program.cs files / two DI containers

- `StockManagement/StockManagement/Program.cs` — server container. Registers **services, repositories, AND server-side `ClientDataServices`** for prerendering.
- `StockManagement/StockManagement.Client/Program.cs` — WASM container. Registers the **real HTTP-backed client data services**.

New services/repositories must be registered in the server `Program.cs`; new client data services must be registered in **both** (server registration enables prerendering, client registration runs in the browser).

### Client data service duality (important gotcha)

There are two implementations of each `IXxxDataService`:
- `StockManagement.Client/Services/XxxDataService.cs` — the real WASM implementation, subclasses `GenericDataService<TEditModel, TResponseModel>` and just sets `ApiControllerName`. This is what actually runs in the browser.
- `StockManagement/StockManagement/ClientDataServices/ClientXxxDataService.cs` — server-side counterpart used during prerender. **Many of these throw `NotImplementedException`** (e.g. `ClientProductDataService`). This is intentional for WASM-only pages; do not "fix" them without understanding whether the page prerenders.

`GenericDataService` centralizes CRUD-over-HTTP, unwraps the `ApiResponse` envelope, and reports failures through `ErrorNotificationService`.

### Dates

Transaction, sale and stock-order dates are **calendar dates, not instants**, and
are stored as `DATE`: `finance.Transaction.Date`, `finance.TransactionDetail.Date`,
`dbo.StockSale.Date`, `dbo.StockOrder.Date`, `finance.InventoryBatch.PurchaseDate`.
`CreateDate` / `AmendDate` remain `DATETIME` — those are genuine audit timestamps.

The client sends dates via `ApiJson.Options` (`StockManagement.Client/Services/ApiJson.cs`),
which serialises `DateTime` with **no timezone offset**. Use it for every
`PostAsJsonAsync` / `PutAsJsonAsync`. Without it, Blazor WASM sends browser-local
time, the UTC container converts it back, and dates entered during British Summer
Time are stored a day early — a bug that silently corrupted 42 sale receipts and
was invisible all winter. Query-string dates go as `yyyy-MM-dd`.

Report date ranges are half-open in SQL: `>= @FromDate AND < DATEADD(DAY, 1, @ToDate)`.
Never `<= DATEADD(DAY, 1, @ToDate)` — that pulls in the day after the period end.

### Finance reports

The year-end reports live in `finance/Stored Procedures/Report_*.sql` and are
surfaced through `ReportController` → `IReportService` → `IReportRepository`.
`Report_ProfitAndLoss` and `Report_BalanceSheet` take an `@Basis` parameter
(1 = accruals, 2 = cash); on the cash basis, stock is expensed when paid for and
every journal that credits the Inventory account is excluded.

`Report_ProfitAndLoss` returns **all amounts positive**, grouped into Income /
Cost of Sales / Expenses. Net profit is `Income - CostOfSales - Expenses`, never
a plain sum of the rows.

Point-in-time stock valuation is rebuilt from `finance.vw_InventoryBatchMovement`,
which reconstructs each batch's quantity from its movement history. That view
deliberately does **not** filter on `Activity.Deleted`: `Activity_Delete` only
soft-deletes the activity row and does not reverse the stock movement, so the
`InventoryBatchActivity` rows remain authoritative.

PDF reports derive from `PdfDocuments/FinanceReportDocument.cs`, which supplies
the shared header, footer and cell styles. `api/Pdf/year-end-pack` merges all of
them into a single document.

### Stored-procedure conventions

- Naming: `dbo.<Entity>_<Action>` — e.g. `Product_Create`, `Product_LoadAll`, `Product_Update`, `Product_Delete`, `Activity_LoadFiltered`.
- Repositories pass an `@Success` (bool) and, for creates, an `@Id` **output parameter**. On `@Success = false` the repository throws `UnauthorizedAccessException`.
- Every mutating proc takes `@CurrentUserId` — the app enforces per-user authorization inside SQL. Controllers resolve the user via `IdentityUserAccessor.GetRequiredUserAsync(HttpContext)` and pass `appUser.Id` down.
- Finance objects live in the `finance` schema and their own `Finance/` sub-namespaces across every project; core stock objects use `dbo`.

### Cross-cutting details

- **Auth**: ASP.NET Core Identity with **custom `UserStore` / `RoleStore` / `CustomUserClaimsPrincipalFactory`** (in `StockManagement/StockManagement/UserManagement/`) backed by the same stored-procedure DB, not EF's default stores. Controllers are `[Authorize]` by default.
- **Culture**: locked to `en-GB` in both `Program.cs` files — dates and currency are UK format. Preserve this when formatting/parsing.
- **Logging**: a custom `DatabaseLoggerProvider` writes logs to the DB (fire-and-forget). `ILogger.LogError` in controllers persists to the error log.
- **PDFs**: generated with **QuestPDF** (Community license, set in host `Program.cs`); documents live in `StockManagement/StockManagement/PdfDocuments/` and are served via `PdfController`.
- **API response envelope**: mutating controller actions return `ApiResponse { Success, CreatedId, ErrorMessage }` (in `Models/InternalObjects/`). Reads return the `ResponseModel` directly.

### Blazor page conventions

- Pages live in `StockManagement.Client/Pages/` and use **code-behind** partial classes: `Xxx.razor` + `Xxx.razor.cs`, often with a `XxxBase` class via `@inherits`.
- Pages declare `@rendermode InteractiveWebAssembly` and `@layout MainLayoutClient`.
- Editing is done through Bootstrap modal components (e.g. `ProductEditModal.razor`); list + modal is the standard CRUD screen pattern.

# The Accounts (Finance) Module

**How double-entry bookkeeping, the chart of accounts, and the year-end reports are implemented in
StockManagement — and what to change when porting the same design into another solution.**

The last section is written specifically for **Property Portfolio Manager**, which has a
portfolio rather than stock, and needs transactions allocated to individual properties.

---

## 1. What the module does

It is a small, self-contained general ledger:

- A **chart of accounts** the user maintains (`finance.Account`), classified by
  **account type** (Asset, Liability, Revenue, Expense, Equity, Long-term Liability).
- Every financial event is a **transaction** with two or more **balanced detail lines**
  (`finance.Transaction` → `finance.TransactionDetail`). Debits and credits always net to zero.
- Some transactions the user enters directly (an expense, an item of income, a journal).
  Others are posted **automatically by other stored procedures** when a source document is
  confirmed (a sale, a stock write-off).
- A set of **reports** reads only the ledger: trial balance, profit and loss, balance sheet,
  nominal ledger, income and expenditure listing, owner's capital and drawings, year-end checks.
- Each report has an on-screen Blazor page and a **QuestPDF** document, and all of them merge into
  a single "year-end pack" PDF to send to an accountant.

Everything sits in the `finance` SQL schema and in `Finance/` sub-namespaces in each project.
The stock-specific parts (`InventoryBatch*`, `Report_StockValuation`, `Report_StockReconciliation`,
`Report_InventoryValue`) also live in `finance` but are **not** part of the ledger core — they are
the inventory valuation that feeds it, and they do not port.

---

## 2. The data model

Four tables carry the whole ledger.

### `finance.AccountType` — the classification
[AccountType.sql](../StockManagement.Database/finance/Tables/AccountType.sql)

| Id | Type | CreditDebit |
|---|---|---|
| 1 | Asset | 1 |
| 2 | Liability | −1 |
| 3 | Revenue | −1 |
| 4 | Expense | 1 |
| 5 | Equity | −1 |
| 6 | Long-term Liability | −1 |

`Id` is **not** an identity — the values are seeded and the reports hard-code them
(`AccountTypeId = 3` means revenue, `= 4` means expense, and so on). Treat the six ids as part of
the schema.

`CreditDebit` is the sign that turns a raw balance into the account's *natural* presentation
(+ for a debit-natured account, − for a credit-natured one). It is used by
`TransactionDetail_LoadTotalFiltered`; the year-end reports do the sign flip explicitly in each
query instead, which is clearer to read.

### `finance.Account` — the chart of accounts
[Account.sql](../StockManagement.Database/finance/Tables/Account.sql)

`Id` (identity), `AccountTypeId`, `Name`, `Notes`, `Active`, `Deleted` + audit columns.
User-maintainable through the **Accounts** page. `Active = 0` hides an account from the entry
drop-downs without losing its history.

### `finance.Transaction` — the journal header
[Transaction.sql](../StockManagement.Database/finance/Tables/Transaction.sql)

`Id`, `TransactionTypeId`, `Date` (**DATE**), `Reference`, `Deleted` + audit.

`TransactionType` is seeded: 1 = Journal, 2 = Expense, 3 = Income, 4 = Sale. `Reference` is
generated in SQL as `PREFIX-yyyymmdd-contactid`, e.g. `EXP-20260331-14`.

### `finance.TransactionDetail` — the journal lines
[TransactionDetail.sql](../StockManagement.Database/finance/Tables/TransactionDetail.sql)

`Id`, `TransactionId`, `AccountId`, `Date` (**DATE**), `Description`, `Amount` (MONEY),
`Direction` (SMALLINT), `ContactId` (nullable) + audit.

**`Direction` is the whole of double entry.** `Amount` is always stored positive; `Direction` is
`1` for a debit and `-1` for a credit. Every derived figure in the system is some form of
`SUM(Amount * Direction)`:

| Want | Expression |
|---|---|
| Net movement on an account (debit-positive) | `SUM(td.Amount * td.Direction)` |
| Revenue / a credit balance as a positive number | `SUM(td.Amount * td.Direction * -1)` |
| Effect on profit | `SUM(td.Amount * td.Direction * -1)` — a revenue credit adds, an expense debit subtracts |
| Debit column of a trial balance | `SUM(CASE WHEN Direction = 1 THEN Amount ELSE 0 END)` |

`Date` is duplicated on header and line. The lines are what every report filters on.

### What is deliberately absent

There is **no bank account and no cash book**. The business has no separate bank account: costs
are paid personally and sale proceeds are banked personally, so the contra side of every
user-entered expense or item of income is the owner's equity, not cash. `finance.BankAccount` and
`finance.BankAccountDetail` exist as **commented-out files** — a design that was started and
abandoned. Do not treat them as live.

There is also no period-close: nothing locks a prior year. Retained earnings are recomputed from
the whole history every time the balance sheet runs.

---

## 3. How transactions get created

Three routes, and the distinction matters when porting.

### 3.1 `Transaction_CreateExpenseIncome` — user-entered expense or income
[Transaction_CreateExpenseIncome.sql](../StockManagement.Database/finance/Stored%20Procedures/Transaction_CreateExpenseIncome.sql)

Takes **one** account plus a type, and writes both sides:

| Type | Chosen account | Direction | Contra account (hard-coded) |
|---|---|---|---|
| Expense (2) | the expense account | debit | 3 — Owner's Capital/Investment (credit) |
| Income (3) | the income account | credit | 4 — Owner's Drawings (debit) |

The user picks a category and an amount; the counterparty is implied. This is only correct
*because* there is no business bank account — an expense paid personally **is** capital
introduced, and income received personally **is** a drawing.

> **This is the single biggest thing to change when porting.** Any solution with a real bank
> account must let the user choose the "paid from / received into" account rather than hard-coding
> accounts 3 and 4. See §9.3.

`Transaction_UpdateExpenseIncome` mirrors this: it locates the paired line by
`TransactionId + AccountId = @AssociatedAccountId` and updates both, and it re-derives the
associated account from the transaction type rather than trusting what is stored.

`Transaction_DeleteByDetailId` soft-deletes the header and **every** line under it, given any one
line id. Deletes are always `Deleted = 1`, never `DELETE`.

### 3.2 `Transaction_Create` — a general two-sided journal
[Transaction_Create.sql](../StockManagement.Database/finance/Stored%20Procedures/Transaction_Create.sql)

Takes `@DebitAccountId` and `@CreditAccountId` explicitly and writes the header plus two lines.
This is the general-purpose posting routine and the one to keep.

### 3.3 Source documents post their own journals, in SQL

`Transaction_Create` is **never called from C#**. It is called from other stored procedures at the
moment a business event is recorded, inside the same transaction:

- [`StockSale_ConfirmSale`](../StockManagement.Database/dbo/Stored%20Procedures/StockSale_ConfirmSale.sql)
  — debit Cost of Goods Sold, credit Inventory, for the cost released by the sale. The resulting
  `TransactionDetailId` is written back onto `StockSaleDetail`, so the source row and its ledger
  posting are linked both ways.
- [`Activity_Create`](../StockManagement.Database/dbo/Stored%20Procedures/Activity_Create.sql)
  — stock deleted or damaged debits Inventory Shrinkage; promotional use debits Advertising &
  Promotion; each credits Inventory.

If the posting fails, the proc `RAISERROR`s and the whole business transaction rolls back. The
ledger cannot drift out of step with the source document.

**Keep this pattern.** In a property system the equivalent is: recording a rent receipt or a
property expense posts its own journal from within the same stored procedure that writes the
source row.

---

## 4. Conventions the module inherits

- **Naming**: `finance.<Entity>_<Action>` — `Account_Create`, `Transaction_CreateExpenseIncome`,
  `Report_TrialBalance`.
- **`@Success BIT OUTPUT`** on every mutating proc; `@Id` / `@TransactionDetailId` output on
  creates. The repository throws `UnauthorizedAccessException` when `@Success = 0`.
- **`@CurrentUserId`** on every mutating proc — authorization is enforced in SQL. Controllers get
  it from `IdentityUserAccessor.GetRequiredUserAsync(HttpContext)`.
- **Soft delete** everywhere; every read filters `Deleted = 0`.
- **Audit columns** `CreateUserId / CreateDate / AmendUserId / AmendDate` on every table.
  `CreateDate` / `AmendDate` are `DATETIME` — genuine timestamps. Business dates are `DATE`.

### Dates — the rule that cost 42 corrupted receipts

Transaction dates are **calendar dates, not instants**:

- Stored as `DATE` (`finance.Transaction.Date`, `finance.TransactionDetail.Date`).
- Passed to Dapper as `DbType.Date` via `ReportRepository.AddDate`, so no stray time component can
  reach the comparison.
- Serialised by the client through
  [`ApiJson.Options`](../StockManagement/StockManagement.Client/Services/ApiJson.cs) on **every**
  `PostAsJsonAsync` / `PutAsJsonAsync`. Blazor WASM runs in browser-local time; without this,
  midnight on 31 March serialises as `2026-03-31T00:00:00+01:00` during British Summer Time, the
  UTC container converts it back to 23:00 on the 30th, and the transaction lands in the wrong day —
  and, at a year end, the wrong year. It was invisible all winter, when the offset is zero.
- Query-string dates go as `yyyy-MM-dd`.

**Report ranges are half-open**: `>= @FromDate AND < DATEADD(DAY, 1, @ToDate)`. Never
`<= DATEADD(DAY, 1, @ToDate)` — that pulls in the day after the period end. Every report declares
`DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);` at the top and uses it.

---

## 5. The layer stack

The module follows the solution's standard vertical slice (see `CLAUDE.md`), with one extra layer
because the client runs in WebAssembly:

```
Blazor page (WASM)
  → IXxxDataService            (HTTP, StockManagement.Client/Services/Finance/)
  → XxxController              (api/[controller], ApiControllers/Finance/)
  → IXxxService                (AutoMapper EditModel ⇄ Dto, Services/Finanace/)
  → IXxxRepository             (Dapper, Repositories/Finance/)
  → finance.Xxx_* stored procedure
```

Note the folder `StockManagement.Services/Finanace/` — the typo is in the real path and in the
namespace `StockManagement.Repositories.Interfaces.Finanace`. Don't reproduce it.

### Files in the ledger core

| Layer | Files |
|---|---|
| Database | `finance/Tables/{Account,AccountType,Transaction,TransactionDetail,TransactionType}.sql`, `finance/Stored Procedures/{Account_*,AccountType_LoadAll,Transaction_*,TransactionDetail_*,Report_*}.sql` |
| Models — DTO | `Dto/Finance/{AccountDto,AccountTypeDto,TransactionDetailDto,AccountingBasis,FinancialYear}.cs` + one DTO per report |
| Models — API | `Finance/{AccountEditModel,AccountResponseModel,AccountTypeEditModel,AccountTypeResponseModel,TransactionDetailEditModel,TransactionDetailResponseModel,TransactionFilterModel,TransactionFilteredResponseModel}.cs` |
| Repositories | `Finance/{AccountRepository,AccountTypeRepository,TransactionRepository}.cs`, `ReportRepository.cs` |
| Services | `Finanace/{AccountService,AccountTypeService,TransactionService}.cs`, `ReportService.cs` |
| API | `ApiControllers/Finance/{AccountController,AccountTypeController,TransactionController}.cs`, `ApiControllers/ReportController.cs`, `ApiControllers/PdfController.cs` |
| Client services | `Client/Services/Finance/{AccountDataService,AccountTypeDataService,TransactionDataService}.cs`, `Client/Services/ReportDataService.cs` |
| Prerender stubs | `ClientDataServices/Finance/Client*DataService.cs`, `ClientDataServices/ClientReportDataService.cs` |
| Pages | `Client/Pages/Finance/*.razor` + `.razor.cs` |
| Components | `Client/Components/Finance/{AccountEditModal,TransactionDetailEditModal,ReportPeriodFilter}.razor` |
| PDF | `PdfDocuments/{FinanceReportDocument,ProfitAndLossDocument,BalanceSheetDocument,…}.cs` |

### Two DI containers

- `StockManagement/StockManagement/Program.cs` — services, repositories, **and** the server-side
  `Client*DataService` stubs (registered so prerendering can resolve them).
- `StockManagement/StockManagement.Client/Program.cs` — the real HTTP-backed data services.

Every client data service is registered in **both**. The server-side stubs mostly
`throw NotImplementedException` (`ClientTransactionDataService` does) — that is intentional,
because the finance pages are WebAssembly-only and never load data during prerender. The pages
guard on `if (JSRuntime is IJSInProcessRuntime)` before fetching, so the stub is never reached.

### Generic CRUD

`AccountDataService` subclasses
[`GenericDataService<TEditModel, TResponseModel>`](../StockManagement/StockManagement.Client/Services/GenericDataService.cs)
and sets `ApiControllerName = "Account"`; that is the whole class. `GenericDataService` handles
CRUD over HTTP, unwraps the `ApiResponse { Success, CreatedId, ErrorMessage }` envelope, and
reports failures through `ErrorNotificationService`.

`TransactionDataService` does **not** subclass it — its operations are
`CreateExpenseIncome` / `UpdateExpenseIncome` / `DeleteByDetailId` / `GetFiltered`, not plain
CRUD — so it builds its own URLs, formatting dates as `yyyy-MM-dd` and posting bodies with
`ApiJson.Options`.

---

## 6. The reports

All in [`finance/Stored Procedures/Report_*.sql`](../StockManagement.Database/finance/Stored%20Procedures/),
reached through `ReportController` → `IReportService` → `IReportRepository`. They are read-only,
take a date range, and touch nothing but the four ledger tables (plus `dbo.Contact` for names).

| Report | Purpose | Notes |
|---|---|---|
| `Report_TrialBalance` | Debit / credit totals and net balance per account for the period | Proof the ledger balances |
| `Report_ProfitAndLoss` | Income / Cost of Sales / Expenses for the period | See the sign rule below |
| `Report_BalanceSheet` | Position **at** `@ToDate` | Cumulative, not period-filtered |
| `Report_NominalLedger` | Every posting in the period by account, with opening balance and running balance | What the accountant works through |
| `Report_IncomeExpenditure` | Line-by-line income and expenditure, flagging non-cash postings | Agrees the P&L to source |
| `Report_OwnersAccount` | Capital introduced and drawings, categorised | The only record of owner ↔ business cash flow |
| `Report_YearEndChecks` | Cut-off exceptions to review before closing | See below |

### The P&L sign rule

`Report_ProfitAndLoss` returns **all amounts positive**, tagged with a `SectionId`
(1 = Income, 2 = Cost of Sales, 3 = Expenses). The caller computes:

```
Gross profit = Income − CostOfSales
Net profit   = Income − CostOfSales − Expenses
```

**Never a plain sum of the rows.** The report previously returned everything positive and the UI
summed it, reporting *revenue plus expenses* as net profit. The rule is now stated in
`ProfitAndLossDto`, in `ProfitAndLossReportBase.NetProfit`, and in `ProfitAndLossDocument`, and all
three must agree.

### The balance sheet

A balance sheet is a **position at a date**, so balances accumulate from the beginning of time to
`@ToDate`. `@FromDate` is used for one thing only: splitting accumulated profit into *retained
earnings brought forward* (before `@FromDate`) and *profit for the period*. Without that split the
sheet does not balance.

Sections: 1 Assets, 2 Liabilities, 3 Long-term Liabilities, 4 Capital. Liabilities and capital are
sign-flipped so they present positive; drawings come through negative. The two computed capital
lines have `AccountId = NULL` and sort last via a synthetic `SortName` of `'zz1'` / `'zz2'`.

### Accounting basis

`Report_ProfitAndLoss` and `Report_BalanceSheet` take `@Basis TINYINT` — 1 = accruals, 2 = cash —
surfaced as [`AccountingBasis`](../StockManagement.Models/Dto/Finance/AccountingBasis.cs).

Under the **cash basis**, stock is expensed when paid for. Every journal that **credits the
Inventory account** (cost of goods sold, shrinkage, damaged, promotional and personal use) is
excluded, and the debits to Inventory — the cash actually paid to suppliers — are expensed
instead. No closing stock is carried on the balance sheet. This is HMRC's default basis for sole
traders from 2024/25.

The exclusion test appears in several places and always takes this form:

```sql
NOT EXISTS (SELECT 1 FROM finance.TransactionDetail x
            WHERE x.TransactionId = td.TransactionId
              AND x.AccountId = 6 AND x.Direction = -1 AND x.Deleted = 0)
```

That is inventory-specific. A property system has no equivalent unless it capitalises something
similar — see §9.6.

### Non-cash flagging

`Report_IncomeExpenditure` returns `IsNonCash` using the same test, so the accountant can see
which lines never involved money. `Report_OwnersAccount` uses it to categorise each movement as
*business cost paid personally*, *sale proceeds received personally*, *stock purchase funded
personally*, or *stock taken for own use (non-cash)*.

### Year-end checks

`Report_YearEndChecks` is the only report that reaches outside `finance` — it reads `dbo.StockSale`
and `dbo.StockOrder`. It exists because **cost and revenue for the same sale are dated
differently**: cost of goods sold is posted at the sale date, revenue at the payment date, so a
sale near the year end can straddle the boundary. It lists those, plus stock gone but never paid
for, and stock paid for but not yet received. All of it is stock-specific; the *idea* ports, the
queries do not.

### Financial year helper

[`FinancialYear`](../StockManagement.Models/Dto/Finance/FinancialYear.cs) encodes the UK personal
tax year, 6 April to 5 April: `Containing(date)`, `MostRecentlyCompleted(today)` — the period the
accounts are usually being prepared for, and the default every report page opens on — and
`Describe(from)` for the "2025/26" label.

**Change this for a company or a different year end.** A property business may use a 31 March or
31 December year end, or a company accounting reference date.

---

## 7. The UI

### Report pages

Every report page inherits
[`FinanceReportBase`](../StockManagement/StockManagement.Client/Pages/Finance/FinanceReportBase.cs),
which supplies:

- `FromDate` / `ToDate`, defaulted to `FinancialYear.MostRecentlyCompleted(DateTime.Today)`
- `Basis`, `PeriodLabel` ("6 April 2025 to 5 April 2026"), `PeriodQuery`
  (`fromDate=…&toDate=…`, shared by the page and its PDF link)
- `IsLoading` handling and the WASM-only load guard
- one abstract method, `LoadReportDataAsync()`

A page is then a dozen lines of C# plus markup:

```csharp
public partial class TrialBalanceReportBase : FinanceReportBase
{
    protected List<TrialBalanceDto> Items = new();

    protected override async Task LoadReportDataAsync()
        => Items = await ReportDataService.GetTrialBalanceReportAsync(FromDate, ToDate);

    protected string PdfUrl => $"api/Pdf/trial-balance?{PeriodQuery}";
}
```

All pages declare `@page`, `@layout MainLayoutClient`, `@rendermode InteractiveWebAssembly`,
`@inherits XxxBase`, and use code-behind partial classes.

### `ReportPeriodFilter`

[`ReportPeriodFilter.razor`](../StockManagement/StockManagement.Client/Components/Finance/ReportPeriodFilter.razor)
is the shared header on every report: from/to date pickers (`ShowFromDate="false"` collapses it to
a single "As at" for point-in-time reports), an optional basis selector (`ShowBasis`), a one-click
**"Tax year 2025/26"** button, and the PDF download link. It raises `OnChanged` after any change
so the page reloads.

### Entry pages

`Expenses` and `Income` are the same screen with a different account type constant
(`AccountTypeId` 4 or 3) and transaction type. Both are list + Bootstrap modal
(`TransactionDetailEditModal`), the standard CRUD pattern. `Transactions` is the filtered,
paginated view over everything, driven by `TransactionFilterModel`
(dates, account, contact, transaction type, page, page size).

One quirk worth knowing: `TransactionDetail_LoadFiltered` treats `@AccountId = -1` as "both owner
equity accounts (3 and 4)", so the UI can offer a single *Capital & Drawings* filter option.

### Navigation

All of it hangs off a collapsible **Finance Options** section in
[`NavMenu.razor`](../StockManagement/StockManagement.Client/Shared/NavMenu.razor) — expenses,
income, transactions, then each report. Routes are kebab-case: `/balance-sheet`,
`/profit-and-loss`, `/trial-balance`, `/nominal-ledger`, `/income-expenditure`,
`/owners-account-report`, `/year-end-checks`.

---

## 8. PDFs

QuestPDF (Community licence, set in the host `Program.cs`).

[`FinanceReportDocument`](../StockManagement/StockManagement/PdfDocuments/FinanceReportDocument.cs)
is the abstract base: A4, 30pt margin, header with business name (from `SettingEnum.BusinessName`),
logo, title, period description and optional sub-heading (the basis); footer with preparation date,
website and page x of y. It also exposes the shared cell styles — `HeaderCell`, `BodyCell`,
`TotalCell`, `SectionCell` — and a UK currency helper.

A derived document supplies `Title`, `PeriodDescription`, optionally `SubHeading`, and
`ComposeBody`. Nothing else.

`PdfController` has one endpoint per report, each a one-liner that calls the report service and
constructs the document, plus **`api/Pdf/year-end-pack`**, which builds all nine documents and
merges them with `Document.Merge(...)` in the order an accountant reads them. That single URL is
the actual deliverable of the module.

---

## 9. Porting to Property Portfolio Manager

### 9.1 What ports unchanged

The entire ledger core: the four tables, `Transaction_Create`, the `Direction` convention, soft
deletes, `@Success` / `@CurrentUserId`, the half-open date ranges, `DATE` columns, `ApiJson`,
`GenericDataService`, `FinanceReportBase`, `ReportPeriodFilter`, `FinanceReportDocument`, the PDF
controller shape and the year-end pack, and the trial balance, nominal ledger, income and
expenditure and owner's account reports.

### 9.2 What to drop

Everything inventory: `InventoryBatch`, `InventoryBatchActivity`, `InventoryBatchStatus`,
`vw_InventoryBatchMovement`, `Report_InventoryValue`, `Report_StockValuation`,
`Report_StockReconciliation`, the Inventory and Cost of Goods Sold accounts, the stock half of
`Report_YearEndChecks`, and the `AccountId = 6` cash-basis exclusion logic. Also drop the
commented-out `BankAccount` / `BankAccountDetail` files rather than carrying them across.

### 9.3 Add a real bank account

Property income and expenditure flow through a bank account, so the StockManagement shortcut —
hard-coding the contra side to owner's equity — must go. Change
`Transaction_CreateExpenseIncome` into something like `Transaction_CreatePayment`, taking both the
category account **and** the funding account:

```
@TransactionTypeId  -- Expense = 2, Income = 3
@AccountId          -- the expense or income category
@PaidFromAccountId  -- bank, credit card, or owner's capital where paid personally
```

with `Direction` on the category line as before and the opposite on the funding line. Seed a
`Bank Current Account` as an Asset. Keep Owner's Capital/Investment and Owner's Drawings — a
portfolio owner still introduces funds and takes drawings — but they become one option among
several rather than the only contra.

Once there is a bank account, an accruals ledger becomes worth having: rent **due** (debit Rent
Receivable, credit Rental Income) separate from rent **received** (debit Bank, credit Rent
Receivable). That is two `Transaction_Create` calls, no new machinery.

### 9.4 Property allocation — the one genuinely new requirement

StockManagement has exactly one analysis dimension on a ledger line: `ContactId`. Property
Portfolio Manager needs a second, and it must be **optional** — a mortgage payment belongs to one
property, an accountancy fee belongs to the portfolio as a whole.

**Put it on the detail line, not the header:**

```sql
ALTER TABLE [finance].[TransactionDetail]
    ADD [PropertyId] INT NULL
        CONSTRAINT [FK_TransactionDetail_Property]
        FOREIGN KEY REFERENCES [dbo].[Property]([Id]);
```

Line-level, because a single invoice can cover several properties, and because the two sides of a
journal are not always allocated the same way (a bank line is portfolio-level even when the
expense line it settles belongs to one property). `NULL` means portfolio-level — do **not** invent
a magic "Portfolio" property row, or every report has to special-case it.

This changes surprisingly little:

- `Transaction_Create` and the payment proc take `@PropertyId INT = NULL` per side and store it.
- `TransactionDetailEditModel` / `Dto` / `ResponseModel` gain `PropertyId` + `PropertyName`,
  and the edit modal gains a property drop-down whose first entry is *"Whole portfolio"*.
- `TransactionFilterModel` gains `PropertyId`, and `TransactionDetail_LoadFiltered` gains the same
  `ISNULL(@PropertyId, 0) = 0 OR td.PropertyId = @PropertyId` pattern already used for
  `@ContactId`.
- Every report proc takes `@PropertyId INT = 0` (0 = whole portfolio) and adds one predicate.
  `ReportPeriodFilter` gains an optional property selector, and `PeriodQuery` gains
  `&propertyId=…` so the PDF link stays in step.

**Decide up front how a property-filtered report treats unallocated lines.** Recommended: a
property-filtered P&L shows only that property's lines and says so in the sub-heading
("Property: 14 Acacia Avenue — excludes unallocated portfolio costs"). Silently apportioning
portfolio overheads across properties is a business decision, not a reporting one; if it is wanted
later, do it as a separate *"P&L by property"* report with an explicit apportionment basis,
leaving the statutory P&L alone.

A **per-property profit and loss** — one column per property plus an unallocated column — is
likely the report the module is really for, and is worth designing in from the start rather than
bolting on.

Note that a property-filtered **balance sheet** does not balance in general: a mortgage is
property-level but the bank account is not. Either restrict property filtering to the P&L and
transaction listings, or accept and label the imbalance. Restricting it is the honest choice.

### 9.5 Chart of accounts

Start from the same six account types (they are generic), and seed something like:

| Type | Accounts |
|---|---|
| Asset | Bank Current Account, Rent Receivable, Tenant Deposits Held, Property at Cost |
| Liability | Accounts Payable, Tenant Deposits (liability), Accruals |
| Long-term Liability | Mortgage — *(one per property, or one with `PropertyId` on the lines)* |
| Revenue | Rental Income, Other Property Income |
| Expense | Mortgage Interest, Repairs & Maintenance, Letting Agent Fees, Insurance, Ground Rent & Service Charge, Utilities, Council Tax, Accountancy & Professional, Bank Charges |
| Equity | Owner's Capital/Investment, Owner's Drawings |

Keep the seeded `AccountType` ids **1–6 as they are** — the report queries hard-code them, and
keeping them identical means the report SQL ports with only the property predicate added.

Mortgage interest and capital repayment must be split at posting time: interest is an expense,
capital repayment reduces the mortgage liability. That is one `Transaction_Create` with three
lines, or two calls — either way, plan for the split rather than posting the whole direct debit to
one account.

### 9.6 Accounting basis

Keep the `@Basis` parameter and `AccountingBasis` enum — cash versus accruals is a live choice for
a UK property business too — but the *implementation* is entirely different. There is no inventory
to exclude. Under the cash basis for property, exclude rent accrued but not received and expenses
accrued but not paid, i.e. lines against the receivable and payable accounts. Decide the exact
rule before writing the SQL; do not port the Inventory test and adapt it.

If a cash basis is not needed on day one, drop `@Basis` entirely rather than shipping a parameter
that is ignored — that is exactly the defect the 02 Aug 2026 rewrite of these procedures had to
fix for `@FromDate`.

### 9.7 Year end

Replace `FinancialYear` with whatever the portfolio's accounting period is. If it is a company
with a 31 March year end, the helper gets simpler; if it stays a UK personal tax year for a
sole-trader landlord, it ports as is.

Rewrite `Report_YearEndChecks` around property-specific cut-off risks rather than stock:
rent due but never received at the year end, deposits held without a matching liability, mortgage
statements not reconciled, expenses dated in the wrong period, properties with no transactions at
all in the year.

### 9.8 Suggested build order

1. Tables + seed data (`AccountType`, `TransactionType`, chart of accounts), with `PropertyId`
   on `TransactionDetail` from the very first migration — retrofitting it later means revisiting
   every report.
2. `Transaction_Create`, the payment proc, update and delete procs.
3. Repository → service → controller → client data service for accounts and transactions;
   register in **both** `Program.cs` files.
4. Accounts, Expenses/Income and Transactions pages with the property selector.
5. `Report_TrialBalance` first — it is the smallest report and it proves the ledger balances.
6. P&L and balance sheet, then the detail listings.
7. `FinanceReportBase` + `ReportPeriodFilter`, then the report pages.
8. `FinanceReportDocument` + one PDF, then the rest, then the year-end pack.

---

## 10. Traps worth restating

1. **Never sum P&L rows.** `Income − CostOfSales − Expenses`. All rows come back positive.
2. **Half-open date ranges.** `< DATEADD(DAY, 1, @ToDate)`, never `<=`.
3. **`ApiJson.Options` on every POST/PUT** carrying a date. Omit it and dates land a day early
   between late March and late October, and nowhere else.
4. **The balance sheet is cumulative to `@ToDate`.** `@FromDate` only splits retained earnings.
5. **A parameter that is accepted but ignored is worse than no parameter.** Both the P&L and the
   balance sheet took `@FromDate` and silently discarded it for a year.
6. **Account type ids are schema.** Reports hard-code 1–6. So are the seeded account ids the
   posting procs reference — document any you hard-code, and prefer looking them up by a stable
   code column if you want to avoid that entirely.
7. **Post journals from the source-document stored procedure, in the same SQL transaction.**
   Never from C#, or the ledger will drift from the documents it is supposed to describe.

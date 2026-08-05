# Point-in-Time Stock Valuation

**How the quantity and value of stock at a given date (e.g. a financial year end) is calculated.**

---

## 1. Why the obvious approach doesn't work

`finance.InventoryBatch` holds one row per batch of stock received, each with its own `UnitCost`
and a `QuantityRemaining` column. `QuantityRemaining` is a **running** figure — it is decremented
as stock is sold, moved or written off, so it only ever tells you what is on hand **now**.

That is what
[`finance.Report_InventoryValue`](../StockManagement.Database/finance/Stored%20Procedures/Report_InventoryValue.sql)
uses, and it is why that report cannot answer "what was stock worth on 5 April?". There is no
historic snapshot of `QuantityRemaining` anywhere in the database.

So the point-in-time reports ignore `QuantityRemaining` entirely and **rebuild each batch's
quantity from its movement history**, stopping the clock at the report date.

---

## 2. Step one — signed movements

[`finance.vw_InventoryBatchMovement`](../StockManagement.Database/finance/Views/vw_InventoryBatchMovement.sql)

This view is the single definition of *what happened to a batch and when*. It joins:

```
finance.InventoryBatchActivity  →  finance.InventoryBatch  →  dbo.Activity  →  dbo.Action
```

and turns every activity row into a **signed quantity**, dated by `Activity.ActivityDate` (the
date the stock actually moved — not `CreateDate`, which is an audit timestamp).

### Sign rules

| `ActionId` | Action | Effect |
|---|---|---|
| 1 | Add new stock | always `+` |
| 2 | Move stock room → location | `−` on the stock room batch, `+` on the destination batch |
| 3 | Return location → stock room | mirror of action 2 |
| 4 | Delete stock | `−` |
| 5 | Sale | `−` |
| 6 | Promotional use | `−` |
| 7 | Damaged stock | `−` |
| 8 | Personal use | `−` |

For actions 2 and 3 a *pair* of `InventoryBatchActivity` rows is written — one against the source
batch and one against the destination batch. The view decides which is which from
`ib.LocationId = 1` (the stock room), so a transfer nets to zero across the two batches and total
stock is unchanged, while stock *by location* moves correctly.

### The invariant

Summing `SignedQty` for a batch across **all** dates reproduces `InventoryBatch.QuantityRemaining`
exactly. That was validated against all 835 live batches when the view was written, and it is the
property that makes the whole approach trustworthy: the movement history and the running total
agree, so cutting the history off at a date gives a genuine historic position.

### Why `Activity.Deleted` is deliberately not filtered

The view filters `InventoryBatchActivity.Deleted = 0` and `InventoryBatch.Deleted = 0`, but
**not** `Activity.Deleted`. This is intentional and must not be "tidied up".

`Activity_Delete` only *soft-deletes* the `dbo.Activity` row — it does not reverse the stock
movement that activity caused. The `InventoryBatchActivity` rows therefore remain the
authoritative record of what happened to the stock. Adding an `Activity.Deleted = 0` filter breaks
the reconciliation on 16 batches, because their stock genuinely moved even though the activity row
was later marked deleted.

---

## 3. Step two — the valuation

[`finance.Report_StockValuation`](../StockManagement.Database/finance/Stored%20Procedures/Report_StockValuation.sql)

```sql
;WITH BatchQty AS (
    SELECT m.InventoryBatchId, m.ProductId, m.ProductTypeId, m.LocationId, m.UnitCost,
           SUM(m.SignedQty) AS Quantity
    FROM finance.vw_InventoryBatchMovement m
    WHERE m.ActivityDate <= @AsAtDate
    GROUP BY m.InventoryBatchId, m.ProductId, m.ProductTypeId, m.LocationId, m.UnitCost
    HAVING SUM(m.SignedQty) > 0
)
```

Three things are doing the work here:

- **`WHERE m.ActivityDate <= @AsAtDate`** — the cut-off. Everything after the as-at date is
  invisible, so the result is the position as it stood at the close of that day.
- **`GROUP BY … m.InventoryBatchId, m.UnitCost`** — grouping is **per batch**, which is what keeps
  FIFO costing intact. Each batch retains the cost it was actually bought at; the query never
  blends batches into an average cost.
- **`HAVING SUM(m.SignedQty) > 0`** — only batches with stock still on hand at that date survive.

The outer query then joins to `Product`, `ProductType` and `Location` and returns, grouped by
location / product type / product:

| Column | Meaning |
|---|---|
| `Quantity` | `Σ` batch quantities on hand at `@AsAtDate` |
| `CostValue` | `Σ Quantity × UnitCost` — **stock at cost (FIFO)** |
| `MarketValue` | `Σ Quantity × ProductType.DefaultSalePrice` |

`MarketValue` exists so the accountant can apply *lower of cost and net realisable value*, and so
that nil-cost items are visible: self-produced originals carry no purchase cost (the materials
were expensed as they were bought), so their cost value is nil while their market value is not.
The Blazor page surfaces those separately as `NilCostQuantity` / `NilCostMarketValue`.

Optional filters `@LocationId`, `@ProductTypeId` and `@ProductId` all take `0` to mean *all*.

---

## 4. Step three — proving the figure

[`finance.Report_StockReconciliation`](../StockManagement.Database/finance/Stored%20Procedures/Report_StockReconciliation.sql)

The valuation above is built from the **stock records**. The reconciliation builds the same closing
figure a second way, from the **ledger**, and compares them.

It takes every `finance.TransactionDetail` line on the Inventory account (`AccountId = 6`) and
classifies it by the account on the *other* side of the journal:

| Counter account | Line |
|---|---|
| — (debits to Inventory) | Add: stock purchased |
| 9 — Cost of Goods Sold | Less: cost of goods sold |
| 10 — Inventory Shrinkage | Less: stock written off / damaged |
| 8 — Advertising & Promotion | Less: stock used for promotion |
| 4 — Owner's Drawings | Less: stock taken for own use |
| anything else | Less: other reductions |

```
Opening stock + purchases − cost of sales − shrinkage − promotional − own use = closing stock (per ledger)
```

The report prints that total ("Closing stock per ledger") next to the physical valuation ("Closing
stock per stock records" — the same `vw_InventoryBatchMovement` CTE, cut off at `@ToDate`) and the
difference between them. **The only difference should ever be FIFO rounding.** Anything larger
means the ledger and the stock records have diverged and needs investigating before the accounts
are signed off.

Note the date handling: the period is half-open —
`>= @FromDate AND < DATEADD(DAY, 1, @ToDate)` — which is the convention across all the finance
reports. Opening stock is everything dated *before* `@FromDate`.

---

## 5. Where the code lives

| Layer | File |
|---|---|
| View | [`finance/Views/vw_InventoryBatchMovement.sql`](../StockManagement.Database/finance/Views/vw_InventoryBatchMovement.sql) |
| Stored procs | [`Report_StockValuation.sql`](../StockManagement.Database/finance/Stored%20Procedures/Report_StockValuation.sql), [`Report_StockReconciliation.sql`](../StockManagement.Database/finance/Stored%20Procedures/Report_StockReconciliation.sql) |
| Repository | [`ReportRepository.GetStockValuationReportAsync`](../StockManagement.Repositories/ReportRepository.cs) — Dapper, uses `AddDate` for the `DATE` parameter |
| Service | [`ReportService`](../StockManagement.Services/ReportService.cs) |
| API | [`ReportController`](../StockManagement/StockManagement/ApiControllers/ReportController.cs) — `GET api/Report/stockvaluation` |
| DTO | [`StockValuationDto`](../StockManagement.Models/Dto/Finance/StockValuationDto.cs) |
| Client service | [`ReportDataService`](../StockManagement/StockManagement.Client/Services/ReportDataService.cs) |
| Page | [`StockValuationReport.razor.cs`](../StockManagement/StockManagement.Client/Pages/Finance/StockValuationReport.razor.cs) — only `ToDate` applies (it is a position, not a period); totals and the nil-cost split are computed client-side |
| PDF | [`StockValuationDocument.cs`](../StockManagement/StockManagement/PdfDocuments/StockValuationDocument.cs), served from `GET api/Pdf/stock-valuation` and included in the year-end pack |

---

## 6. Caveats worth knowing

1. **`HAVING SUM(...) > 0` silently drops any batch that nets negative** at the as-at date. That is
   the right behaviour for a valuation — you cannot hold negative stock — but it means a data error
   would quietly disappear rather than show up as a negative line. The stock reconciliation is the
   safety net for this.
2. **Batch status is not filtered.** The view exposes `InventoryBatchStatusId` but
   `Report_StockValuation` ignores it, so a *pending* batch is counted as soon as it has an "add
   new stock" activity dated on or before the as-at date. This differs from
   `Report_InventoryValue`, which deliberately returns active and active+pending as two separate
   totals.
3. **Market value is a *current* price.** `ProductType.DefaultSalePrice` is read as it stands
   today, not as it stood at the as-at date. Cost value is properly historic; market value is not.
   For a year end more than a price-change away, treat `MarketValue` as indicative.
4. **Everything hangs off `Activity.ActivityDate`.** Back-dating or correcting an activity date
   changes historic valuations, including ones already reported to the accountant.

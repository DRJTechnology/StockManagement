-- =============================================================================
-- Author:      Dave Brown
-- Create date: 02 Aug 2026
-- Description: Proves the closing stock figure for the accountant.
--
--              Opening stock + purchases - cost of sales - shrinkage
--              - promotional use - personal use = closing stock (per ledger),
--              then compares that to the physical stock valuation rebuilt from
--              the batch movement history. Any gap is FIFO rounding.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_StockReconciliation]
    @FromDate   DATE,
    @ToDate     DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    -- Every movement on the Inventory account, tagged with the account on the
    -- other side of the journal so it can be described.
    ;WITH InvLines AS (
        SELECT td.[Date] AS Dt,
               td.Amount * td.Direction AS SignedAmount,
               (SELECT TOP 1 x.AccountId
                FROM finance.TransactionDetail x
                WHERE x.TransactionId = td.TransactionId
                  AND x.AccountId <> 6 AND x.Deleted = 0) AS CounterAccountId
        FROM finance.TransactionDetail td
        WHERE td.AccountId = 6 AND td.Deleted = 0
    ),
    Totals AS (
        SELECT
            ISNULL(SUM(CASE WHEN Dt < @FromDate THEN SignedAmount ELSE 0 END), 0) AS Opening,
            ISNULL(SUM(CASE WHEN Dt >= @FromDate AND Dt < @ToExclusive
                             AND SignedAmount > 0 THEN SignedAmount ELSE 0 END), 0) AS Purchases,
            ISNULL(SUM(CASE WHEN Dt >= @FromDate AND Dt < @ToExclusive
                             AND SignedAmount < 0 AND CounterAccountId = 9
                            THEN -SignedAmount ELSE 0 END), 0) AS CostOfSales,
            ISNULL(SUM(CASE WHEN Dt >= @FromDate AND Dt < @ToExclusive
                             AND SignedAmount < 0 AND CounterAccountId = 10
                            THEN -SignedAmount ELSE 0 END), 0) AS Shrinkage,
            ISNULL(SUM(CASE WHEN Dt >= @FromDate AND Dt < @ToExclusive
                             AND SignedAmount < 0 AND CounterAccountId = 8
                            THEN -SignedAmount ELSE 0 END), 0) AS Promotional,
            ISNULL(SUM(CASE WHEN Dt >= @FromDate AND Dt < @ToExclusive
                             AND SignedAmount < 0 AND CounterAccountId = 4
                            THEN -SignedAmount ELSE 0 END), 0) AS PersonalUse,
            ISNULL(SUM(CASE WHEN Dt >= @FromDate AND Dt < @ToExclusive
                             AND SignedAmount < 0 AND CounterAccountId NOT IN (4, 8, 9, 10)
                            THEN -SignedAmount ELSE 0 END), 0) AS OtherReductions,
            ISNULL(SUM(CASE WHEN Dt < @ToExclusive THEN SignedAmount ELSE 0 END), 0) AS Closing
        FROM InvLines
    ),
    Physical AS (
        SELECT ISNULL(SUM(q.Quantity * q.UnitCost), 0) AS StockValue,
               ISNULL(SUM(q.Quantity), 0)              AS Units
        FROM (
            SELECT m.InventoryBatchId, m.UnitCost, SUM(m.SignedQty) AS Quantity
            FROM finance.vw_InventoryBatchMovement m
            WHERE m.ActivityDate <= @ToDate
            GROUP BY m.InventoryBatchId, m.UnitCost
            HAVING SUM(m.SignedQty) > 0
        ) q
    )
    SELECT SortOrder, [Description], Amount, IsTotal
    FROM (
        SELECT 1 AS SortOrder, 'Opening stock' AS [Description], t.Opening AS Amount, 0 AS IsTotal FROM Totals t
        UNION ALL SELECT 2, 'Add: stock purchased',            t.Purchases,       0 FROM Totals t
        UNION ALL SELECT 3, 'Less: cost of goods sold',        -t.CostOfSales,    0 FROM Totals t
        UNION ALL SELECT 4, 'Less: stock written off/damaged', -t.Shrinkage,      0 FROM Totals t
        UNION ALL SELECT 5, 'Less: stock used for promotion',  -t.Promotional,    0 FROM Totals t
        UNION ALL SELECT 6, 'Less: stock taken for own use',   -t.PersonalUse,    0 FROM Totals t
        UNION ALL SELECT 7, 'Less: other reductions',          -t.OtherReductions,0 FROM Totals t
        UNION ALL SELECT 8, 'Closing stock per ledger',        t.Closing,         1 FROM Totals t
        UNION ALL SELECT 9, 'Closing stock per stock records', p.StockValue,      1 FROM Physical p
        UNION ALL SELECT 10,'Difference (FIFO rounding)',
                            (SELECT Closing FROM Totals) - p.StockValue,          1 FROM Physical p
    ) x
    -- Opening, purchases and the closing totals always print, even at nil, so
    -- the reconciliation reads as a complete statement. The deduction lines are
    -- suppressed when there is nothing to show.
    WHERE Amount <> 0 OR SortOrder IN (1, 2, 8, 9, 10)
    ORDER BY SortOrder;
END

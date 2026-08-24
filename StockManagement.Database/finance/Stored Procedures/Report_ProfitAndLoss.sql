-- =============================================================================
-- Author:      Dave Brown
-- Create date: 01 Oct 2025
-- Description: Profit and Loss Report
-- =============================================================================
-- 02 Aug 2026 - Rewritten for year-end accounts:
--               * @FromDate / @ToDate are now honoured (were ignored by the
--                 caller) and the closing bound is exclusive - the previous
--                 "<= DATEADD(DAY,1,@ToDate)" pulled in the day after year end.
--               * Rows are returned grouped into Income / Cost of Sales /
--                 Expenses with always-positive amounts, so the caller computes
--                 Income - Cost of Sales - Expenses. Previously every row came
--                 back positive and the UI summed them, reporting
--                 Revenue + Expenses as "Net Profit".
--               * @Basis added: 1 = Accruals, 2 = Cash.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_ProfitAndLoss]
    @FromDate   DATE,
    @ToDate     DATE,
    @Basis      TINYINT = 1     -- 1 = Accruals, 2 = Cash
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    -- A transaction line is non-cash (stock-derived) when the same transaction
    -- credits the Inventory account: cost of goods sold, shrinkage, damaged
    -- stock, promotional use and owner's personal use all take this form.
    -- Under the cash basis these fall out entirely and the cash actually paid
    -- to suppliers (the debits to Inventory) is expensed instead.

    IF @Basis = 2   -- ---------------------------------------------- CASH BASIS
    BEGIN
        SELECT 1 AS SectionId, 'Income' AS Section, a.Id AS AccountId, a.[Name] AS AccountName,
               SUM(td.Amount * td.Direction * -1) AS Amount
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0 AND a.AccountTypeId = 3
          AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
        GROUP BY a.Id, a.[Name]
        HAVING SUM(td.Amount * td.Direction * -1) <> 0

        UNION ALL

        -- Stock bought and paid for in the period. Debits only: the credits to
        -- Inventory are the non-cash consumption journals (cost of sales,
        -- shrinkage, promotional and personal use), which have no cash effect.
        SELECT 2, 'Cost of Sales', a.Id, 'Stock purchases',
               SUM(td.Amount)
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0 AND td.AccountId = 6 AND td.Direction = 1
          AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
        GROUP BY a.Id
        HAVING SUM(td.Amount) <> 0

        UNION ALL

        -- Cash-paid overheads only
        SELECT 3, 'Expenses', a.Id, a.[Name],
               SUM(td.Amount * td.Direction)
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0 AND a.AccountTypeId = 4
          AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
          AND NOT EXISTS (SELECT 1 FROM finance.TransactionDetail x
                          WHERE x.TransactionId = td.TransactionId
                            AND x.AccountId = 6 AND x.Direction = -1 AND x.Deleted = 0)
        GROUP BY a.Id, a.[Name]
        HAVING SUM(td.Amount * td.Direction) <> 0

        ORDER BY SectionId, AccountName;
    END
    ELSE            -- ------------------------------------------ ACCRUALS BASIS
    BEGIN
        SELECT 1 AS SectionId, 'Income' AS Section, a.Id AS AccountId, a.[Name] AS AccountName,
               SUM(td.Amount * td.Direction * -1) AS Amount
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0 AND a.AccountTypeId = 3
          AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
        GROUP BY a.Id, a.[Name]
        HAVING SUM(td.Amount * td.Direction * -1) <> 0

        UNION ALL

        SELECT 2, 'Cost of Sales', a.Id, a.[Name],
               SUM(td.Amount * td.Direction)
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0 AND td.AccountId = 9   -- Cost of Goods Sold
          AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
        GROUP BY a.Id, a.[Name]
        HAVING SUM(td.Amount * td.Direction) <> 0

        UNION ALL

        SELECT 3, 'Expenses', a.Id, a.[Name],
               SUM(td.Amount * td.Direction)
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0 AND a.AccountTypeId = 4 AND td.AccountId <> 9
          AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
        GROUP BY a.Id, a.[Name]
        HAVING SUM(td.Amount * td.Direction) <> 0

        ORDER BY SectionId, AccountName;
    END
END

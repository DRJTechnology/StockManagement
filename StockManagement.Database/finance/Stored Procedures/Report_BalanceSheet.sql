-- =============================================================================
-- Author:      Dave Brown
-- Create date: 20 Sep 2025
-- Description: Balance Sheet Report
-- =============================================================================
-- 23 Sep 2025 - Dave Brown - Ignore deleted transactions
-- 02 Aug 2026 - Rewritten for year-end accounts:
--               * Balances are now cumulative to @ToDate. Previously @FromDate
--                 filtered the balances too, which is meaningless on a balance
--                 sheet (it is a position at a date, not a period).
--               * Returns structured sections (Assets / Liabilities / Capital)
--                 instead of a flat list that mixed in Revenue and Expense
--                 accounts with no subtotals and no retained earnings.
--               * @FromDate is used only to split accumulated profit into
--                 "brought forward" and "for the period", so the sheet balances.
--               * @Basis added: 1 = Accruals, 2 = Cash.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_BalanceSheet]
    @FromDate   DATE,
    @ToDate     DATE,
    @Basis      TINYINT = 1     -- 1 = Accruals, 2 = Cash
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    -- ------------------------------------------------------------------------
    -- Accumulated profit, split brought-forward vs this period.
    -- Profit effect of any line is Amount * Direction * -1: a revenue credit
    -- increases profit, an expense debit reduces it. Under the cash basis the
    -- stock journals drop out and the cash paid for stock is expensed instead.
    -- ------------------------------------------------------------------------
    DECLARE @RetainedBf MONEY, @ProfitPeriod MONEY;

    ;WITH ProfitLines AS (
        SELECT td.[Date] AS Dt, td.Amount * td.Direction * -1 AS ProfitEffect
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0
          AND (
                (@Basis = 1 AND a.AccountTypeId IN (3, 4))
             OR (@Basis = 2 AND (
                     (a.AccountTypeId IN (3, 4)
                      AND NOT EXISTS (SELECT 1 FROM finance.TransactionDetail x
                                      WHERE x.TransactionId = td.TransactionId
                                        AND x.AccountId = 6 AND x.Direction = -1
                                        AND x.Deleted = 0))
                  -- Stock purchases are debits to Inventory. The credits are
                  -- the non-cash consumption journals and have no cash effect.
                  OR (td.AccountId = 6 AND td.Direction = 1)
                 ))
              )
    )
    SELECT @RetainedBf   = ISNULL(SUM(CASE WHEN Dt <  @FromDate THEN ProfitEffect ELSE 0 END), 0),
           @ProfitPeriod = ISNULL(SUM(CASE WHEN Dt >= @FromDate AND Dt < @ToExclusive
                                           THEN ProfitEffect ELSE 0 END), 0)
    FROM ProfitLines
    WHERE Dt < @ToExclusive;

    -- ------------------------------------------------------------------------
    -- Balance sheet lines
    -- ------------------------------------------------------------------------
    ;WITH Balances AS (
        SELECT a.AccountTypeId, a.Id AS AccountId, a.[Name] AS AccountName,
               SUM(td.Amount * td.Direction) AS DebitBalance
        FROM finance.TransactionDetail td
        INNER JOIN finance.Account a ON td.AccountId = a.Id
        WHERE td.Deleted = 0
          AND a.AccountTypeId IN (1, 2, 5, 6)
          AND td.[Date] < @ToExclusive
          -- Under the cash basis stock is expensed when bought, so it is not
          -- carried as an asset, and stock taken for personal use is not a
          -- drawing (its cost was already relieved on purchase).
          AND (@Basis = 1
               OR (td.AccountId <> 6
                   AND NOT EXISTS (SELECT 1 FROM finance.TransactionDetail x
                                   WHERE x.TransactionId = td.TransactionId
                                     AND x.AccountId = 6 AND x.Direction = -1
                                     AND x.Deleted = 0)))
        GROUP BY a.AccountTypeId, a.Id, a.[Name]
    )
    SELECT SectionId, Section, AccountId, AccountName, Amount
    FROM (
        SELECT 1 AS SectionId, 'Assets' AS Section, AccountId, AccountName,
               DebitBalance AS Amount, AccountName AS SortName
        FROM Balances WHERE AccountTypeId = 1

        UNION ALL
        SELECT 2, 'Liabilities', AccountId, AccountName, DebitBalance * -1, AccountName
        FROM Balances WHERE AccountTypeId = 2

        UNION ALL
        SELECT 3, 'Long-term Liabilities', AccountId, AccountName, DebitBalance * -1, AccountName
        FROM Balances WHERE AccountTypeId = 6

        UNION ALL
        SELECT 4, 'Capital', AccountId, AccountName, DebitBalance * -1, AccountName
        FROM Balances WHERE AccountTypeId = 5

        UNION ALL
        SELECT 4, 'Capital', NULL, 'Retained earnings brought forward', @RetainedBf, 'zz1'
        WHERE @RetainedBf <> 0

        UNION ALL
        SELECT 4, 'Capital', NULL, 'Profit for the period', @ProfitPeriod, 'zz2'
    ) x
    WHERE Amount <> 0
    ORDER BY SectionId, SortName;
END

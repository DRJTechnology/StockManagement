-- =============================================================================
-- Author:      Dave Brown
-- Create date: 02 Aug 2026
-- Description: Nominal ledger - every posting in the period, by account, with
--              an opening balance line and a running balance. This is what the
--              accountant works through to tie the accounts back to source.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_NominalLedger]
    @FromDate   DATE,
    @ToDate     DATE,
    @AccountId  INT = 0     -- 0 = all accounts
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    ;WITH Opening AS (
        SELECT td.AccountId, SUM(td.Amount * td.Direction) AS OpeningBalance
        FROM finance.TransactionDetail td
        WHERE td.Deleted = 0 AND td.[Date] < @FromDate
        GROUP BY td.AccountId
    ),
    Lines AS (
        SELECT
            a.AccountTypeId,
            act.[Type]      AS AccountType,
            a.Id            AS AccountId,
            a.[Name]        AS AccountName,
            0               AS LineType,        -- opening balance
            CAST(NULL AS DATE)          AS [Date],
            CAST('' AS NVARCHAR(256))   AS Reference,
            CAST('Opening balance' AS NVARCHAR(512)) AS [Description],
            CAST('' AS NVARCHAR(100))   AS ContactName,
            CAST(0 AS MONEY)            AS Debit,
            CAST(0 AS MONEY)            AS Credit,
            ISNULL(o.OpeningBalance, 0) AS RunningBalance,
            CAST(0 AS INT)              AS SortId
        FROM finance.Account a
        INNER JOIN finance.AccountType act ON a.AccountTypeId = act.Id
        LEFT JOIN Opening o ON o.AccountId = a.Id
        WHERE (@AccountId = 0 OR a.Id = @AccountId)
          AND (ISNULL(o.OpeningBalance, 0) <> 0
               OR EXISTS (SELECT 1 FROM finance.TransactionDetail td
                          WHERE td.AccountId = a.Id AND td.Deleted = 0
                            AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive))

        UNION ALL

        SELECT
            a.AccountTypeId,
            act.[Type],
            a.Id,
            a.[Name],
            1,
            CAST(td.[Date] AS DATE),
            t.Reference,
            td.[Description],
            ISNULL(c.[Name], ''),
            CASE WHEN td.Direction =  1 THEN td.Amount ELSE 0 END,
            CASE WHEN td.Direction = -1 THEN td.Amount ELSE 0 END,
            ISNULL(o.OpeningBalance, 0)
                + SUM(td.Amount * td.Direction) OVER (
                    PARTITION BY a.Id ORDER BY td.[Date], td.Id
                    ROWS UNBOUNDED PRECEDING),
            td.Id
        FROM finance.TransactionDetail td
        INNER JOIN finance.[Transaction] t   ON td.TransactionId = t.Id
        INNER JOIN finance.Account a         ON td.AccountId = a.Id
        INNER JOIN finance.AccountType act   ON a.AccountTypeId = act.Id
        LEFT JOIN dbo.Contact c              ON td.ContactId = c.Id
        LEFT JOIN Opening o                  ON o.AccountId = a.Id
        WHERE td.Deleted = 0
          AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
          AND (@AccountId = 0 OR a.Id = @AccountId)
    )
    SELECT AccountTypeId, AccountType, AccountId, AccountName, LineType,
           [Date], Reference, [Description], ContactName, Debit, Credit, RunningBalance
    FROM Lines
    ORDER BY AccountTypeId, AccountName, LineType, [Date], SortId;
END

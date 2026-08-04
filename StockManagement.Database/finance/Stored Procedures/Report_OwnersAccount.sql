-- =============================================================================
-- Author:      Dave Brown
-- Create date: 02 Aug 2026
-- Description: Owner's capital and drawings account for the period.
--
--              The business has no separate bank account: money the owner puts
--              in shows as capital introduced, money taken out (including sale
--              receipts banked personally) shows as drawings. The accountant
--              needs this in full because it is the only record of cash flow
--              between the owner and the business.
--
--              Category separates real cash movements from the non-cash
--              journal for stock taken for own use, which is a drawing in
--              substance but never involved money.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_OwnersAccount]
    @FromDate   DATE,
    @ToDate     DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    ;WITH Opening AS (
        SELECT td.AccountId, SUM(td.Amount * td.Direction * -1) AS OpeningBalance
        FROM finance.TransactionDetail td
        WHERE td.Deleted = 0 AND td.AccountId IN (3, 4) AND td.[Date] < @FromDate
        GROUP BY td.AccountId
    )
    SELECT
        a.Id                AS AccountId,
        a.[Name]            AS AccountName,
        ISNULL(o.OpeningBalance, 0) AS OpeningBalance,
        CAST(td.[Date] AS DATE) AS [Date],
        t.Reference,
        td.[Description],
        ISNULL(c.[Name], '') AS ContactName,
        -- Credit to capital = money in; debit to drawings = money out.
        td.Amount * td.Direction * -1 AS Amount,
        CASE
            WHEN EXISTS (SELECT 1 FROM finance.TransactionDetail x
                         WHERE x.TransactionId = td.TransactionId
                           AND x.AccountId = 6 AND x.Direction = -1 AND x.Deleted = 0)
                 THEN 'Stock taken for own use (non-cash)'
            WHEN EXISTS (SELECT 1 FROM finance.TransactionDetail x
                         WHERE x.TransactionId = td.TransactionId
                           AND x.AccountId = 6 AND x.Direction = 1 AND x.Deleted = 0)
                 THEN 'Stock purchase funded personally'
            WHEN a.Id = 3 THEN 'Business cost paid personally'
            ELSE 'Sale proceeds received personally'
        END AS Category
    FROM finance.TransactionDetail td
    INNER JOIN finance.[Transaction] t ON td.TransactionId = t.Id
    INNER JOIN finance.Account a       ON td.AccountId = a.Id
    LEFT JOIN dbo.Contact c            ON td.ContactId = c.Id
    LEFT JOIN Opening o                ON o.AccountId = a.Id
    WHERE td.Deleted = 0
      AND td.AccountId IN (3, 4)   -- Owner's Capital/Investment, Owner's Drawings
      AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
    ORDER BY a.Id, td.[Date], td.Id;
END

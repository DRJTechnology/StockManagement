-- =============================================================================
-- Author:      Dave Brown
-- Create date: 02 Aug 2026
-- Description: Line-by-line income and expenditure for the period. This is the
--              listing the accountant reviews to agree the P&L to source.
--
--              IsNonCash flags postings that never involved money changing
--              hands - cost of goods sold, stock written off, stock used for
--              promotion. These are the lines that drop out under the cash
--              basis, and the ones most likely to need explaining.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_IncomeExpenditure]
    @FromDate   DATE,
    @ToDate     DATE,
    @SectionId  TINYINT = 0   -- 0 = both, 1 = income only, 2 = expenditure only
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    SELECT
        CASE WHEN a.AccountTypeId = 3 THEN 1 ELSE 2 END AS SectionId,
        CASE WHEN a.AccountTypeId = 3 THEN 'Income' ELSE 'Expenditure' END AS Section,
        a.Id                AS AccountId,
        a.[Name]            AS AccountName,
        CAST(td.[Date] AS DATE) AS [Date],
        t.Reference,
        td.[Description],
        ISNULL(c.[Name], '') AS ContactName,
        td.Amount * td.Direction * CASE WHEN a.AccountTypeId = 3 THEN -1 ELSE 1 END AS Amount,
        CAST(CASE WHEN EXISTS (SELECT 1 FROM finance.TransactionDetail x
                               WHERE x.TransactionId = td.TransactionId
                                 AND x.AccountId = 6 AND x.Direction = -1
                                 AND x.Deleted = 0)
                  THEN 1 ELSE 0 END AS BIT) AS IsNonCash
    FROM finance.TransactionDetail td
    INNER JOIN finance.[Transaction] t ON td.TransactionId = t.Id
    INNER JOIN finance.Account a       ON td.AccountId = a.Id
    LEFT JOIN dbo.Contact c            ON td.ContactId = c.Id
    WHERE td.Deleted = 0
      AND a.AccountTypeId IN (3, 4)
      AND td.[Date] >= @FromDate AND td.[Date] < @ToExclusive
      AND (@SectionId = 0
           OR (@SectionId = 1 AND a.AccountTypeId = 3)
           OR (@SectionId = 2 AND a.AccountTypeId = 4))
    ORDER BY SectionId, a.[Name], td.[Date], td.Id;
END

-- =============================================================================
-- Author:      Dave Brown
-- Create date: 01 Oct 2025
-- Description: Trial Balance Report
-- =============================================================================
-- 02 Aug 2026 - @FromDate / @ToDate are now honoured (were ignored by the
--               caller) and the closing bound is exclusive - the previous
--               "<= DATEADD(DAY,1,@ToDate)" included the day after year end.
--               Net balance columns added so the report can be read directly.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_TrialBalance]
    @FromDate   DATE,
    @ToDate     DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    SELECT
        act.Id      AS AccountTypeId,
        a.Id        AS AccountId,
        act.[Type]  AS AccountType,
        a.[Name]    AS AccountName,
        SUM(CASE WHEN td.Direction =  1 THEN td.Amount ELSE 0 END) AS Debit,
        SUM(CASE WHEN td.Direction = -1 THEN td.Amount ELSE 0 END) AS Credit,
        CASE WHEN SUM(td.Amount * td.Direction) > 0
             THEN SUM(td.Amount * td.Direction) ELSE 0 END AS BalanceDebit,
        CASE WHEN SUM(td.Amount * td.Direction) < 0
             THEN SUM(td.Amount * td.Direction) * -1 ELSE 0 END AS BalanceCredit
    FROM finance.TransactionDetail td
    INNER JOIN finance.Account a       ON td.AccountId = a.Id
    INNER JOIN finance.AccountType act ON a.AccountTypeId = act.Id
    WHERE td.Deleted = 0
      AND td.[Date] >= @FromDate
      AND td.[Date] <  @ToExclusive
    GROUP BY act.Id, a.Id, act.[Type], a.[Name]
    HAVING SUM(CASE WHEN td.Direction =  1 THEN td.Amount ELSE 0 END) <> 0
        OR SUM(CASE WHEN td.Direction = -1 THEN td.Amount ELSE 0 END) <> 0
    ORDER BY act.Id, a.[Name];
END

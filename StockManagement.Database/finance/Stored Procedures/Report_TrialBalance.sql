-- =======================================================================
-- Author:		Dave Brown
-- Create date: 01 Oct 2025
-- Description:	Trial Balance Report
-- =======================================================================
CREATE PROCEDURE [finance].[Report_TrialBalance]
	@FromDate	datetime = '2000-01-01',
	@ToDate		datetime = '2099-12-31'
AS
BEGIN
	SET NOCOUNT ON;

	-- Use a local variable for adjusted ToDate
	DECLARE @ToDateAdjusted datetime;
	SET @ToDateAdjusted = DATEADD(DAY, 1, CONVERT(date, @ToDate));

	SELECT
		act.Id AS AccountTypeId,
		a.Id AS AccountId,
		act.[Type] AS AccountType,
		a.[Name] AS AccountName,
		SUM(CASE WHEN td.Direction = 1 THEN td.Amount ELSE 0 END) AS Debit,
		SUM(CASE WHEN td.Direction = -1 THEN td.Amount ELSE 0 END) AS Credit
	FROM
		finance.TransactionDetail td
		INNER JOIN finance.Account a ON td.AccountId = a.Id
		INNER JOIN finance.AccountType act ON a.AccountTypeId = act.Id
	WHERE
		td.[Date] >= @FromDate
		AND td.[Date] <= @ToDateAdjusted
		AND td.Deleted = 0
	GROUP BY
		act.Id, a.Id, act.[Type], a.[Name]
	ORDER BY
		act.[Type], a.[Name]
END

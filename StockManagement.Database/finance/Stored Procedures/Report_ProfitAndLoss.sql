-- =======================================================================
-- Author:		Dave Brown
-- Create date: 01 Oct 2025
-- Description:	Profit and Loss Report
-- =======================================================================
CREATE PROCEDURE [finance].[Report_ProfitAndLoss]
	@FromDate	datetime = '2000-01-01',
	@ToDate		datetime = '2099-12-31'
AS
BEGIN
	SET NOCOUNT ON;

	-- Use a local variable for adjusted ToDate
	DECLARE @ToDateAdjusted datetime;
	SET @ToDateAdjusted = DATEADD(DAY, 1, CONVERT(date, @ToDate));

	-- Return only Income (Revenue) and Expense accounts
	SELECT
		act.Id AS AccountTypeId,
		a.Id AS AccountId,
		act.[Type] AS AccountType,
		a.[Name] AS AccountName,
		SUM(td.Amount * td.Direction * act.CreditDebit) AS Balance
	FROM
		finance.TransactionDetail td
		INNER JOIN finance.Account a ON td.AccountId = a.Id
		INNER JOIN finance.AccountType act ON a.AccountTypeId = act.Id
	WHERE
		td.[Date] >= @FromDate
		AND td.[Date] <= @ToDateAdjusted
		AND td.Deleted = 0
		AND act.Id IN (3 /*Revenue*/, 4 /*Expense*/)
	GROUP BY
		act.Id, a.Id, act.[Type], a.[Name]
	ORDER BY
		act.[Type], a.[Name]
END

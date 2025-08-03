-- ==========================================================
-- Author:		Dave Brown
-- Create date: 22 Jul 2025
-- Description:	Load transaction records by account type
-- ==========================================================
CREATE PROCEDURE [finance].[TransactionDetail_LoadByAccountType]
	@AccountTypeId		int,
	@FromDate			datetime = null,
	@ToDate				datetime = null
AS

	SELECT	td.Id,
			td.TransactionId,
			tt.[Type],
			td.[Date], 
			t.Reference,
			td.AccountId, 
			a.[Name] AS Account, 
			td.[Description],
			td.Amount,
			CASE WHEN Direction = 1 THEN Amount ELSE 0 END AS Debit, 
			CASE WHEN Direction = -1 THEN Amount ELSE 0 END AS Credit,
			td.ContactId,
			COALESCE(c.[Name], '') AS ContactName
	FROM	finance.TransactionDetail td
	INNER JOIN	finance.[Transaction] t ON td.TransactionId = t.Id
	INNER JOIN	finance.[TransactionType] tt ON t.TransactionTypeId = tt.Id
	INNER JOIN	finance.Account a ON td.AccountId = a.Id
	LEFT OUTER JOIN dbo.Contact c on td.ContactId = c.Id
	WHERE	a.AccountTypeId = @AccountTypeId
		AND (@FromDate IS NULL OR td.[Date] >= @FromDate)
		AND (@ToDate IS NULL OR td.[Date] < DATEADD(DAY, 1, @ToDate))
		AND td.Deleted = 0
	ORDER BY	td.[Date] DESC, td.TransactionId, a.[Name], Credit, Debit

RETURN 0
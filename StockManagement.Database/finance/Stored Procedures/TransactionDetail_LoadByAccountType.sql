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
			CASE WHEN Direction = 1 THEN Amount ELSE 0 END AS Debit, 
			CASE WHEN Direction = -1 THEN Amount ELSE 0 END AS Credit
	FROM	finance.TransactionDetail td
	INNER JOIN	finance.[Transaction] t ON td.TransactionId = t.Id
	INNER JOIN	finance.[TransactionType] tt ON t.TransactionTypeId = tt.Id
	INNER JOIN	finance.Account a ON td.AccountId = a.Id
	WHERE	a.AccountTypeId = @AccountTypeId
		AND (@FromDate IS NULL OR td.[Date] >= @FromDate)
		AND (@ToDate IS NULL OR td.[Date] < DATEADD(DAY, 1, @ToDate))
	ORDER BY	td.[Date] DESC, td.TransactionId, a.[Name], Credit, Debit

RETURN 0
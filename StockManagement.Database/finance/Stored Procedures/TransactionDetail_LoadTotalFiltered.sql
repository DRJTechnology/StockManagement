-- ==========================================================
-- Author:		Dave Brown
-- Create date: 05 Aug 2025
-- Description:	Load total transaction amount
-- ==========================================================
CREATE PROCEDURE [finance].[TransactionDetail_LoadTotalFiltered]
	@FromDate			datetime = null,
	@ToDate				datetime = null,
	@AccountId			int = null,
	@ContactId			int = null,
	@TransactionTypeId	int = null,
	@TotalAmount		money OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Err int;

	SELECT	@TotalAmount = SUM(td.Amount * td.Direction * act.CreditDebit)
	FROM	finance.TransactionDetail td
	INNER JOIN	finance.[Transaction] t ON td.TransactionId = t.Id
	INNER JOIN	finance.[TransactionType] tt ON t.TransactionTypeId = tt.Id
	INNER JOIN	finance.Account a ON td.AccountId = a.Id
	INNER JOIN	finance.AccountType act ON a.AccountTypeId = act.Id
	LEFT OUTER JOIN dbo.Contact c on td.ContactId = c.Id
	WHERE	(@FromDate IS NULL OR td.[Date] >= @FromDate)
		AND (@ToDate IS NULL OR td.[Date] < DATEADD(DAY, 1, @ToDate))
		AND (ISNULL(@AccountId, 0) = 0 OR td.AccountId = @AccountId)
		AND (ISNULL(@ContactId, 0) = 0 OR td.ContactId = @ContactId)
		AND (ISNULL(@TransactionTypeId, 0) = 0 OR t.TransactionTypeId = @TransactionTypeId)
		AND td.Deleted = 0;

	RETURN 0

END
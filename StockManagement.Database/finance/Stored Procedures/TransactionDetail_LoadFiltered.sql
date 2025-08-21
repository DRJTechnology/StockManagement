-- ==========================================================
-- Author:		Dave Brown
-- Create date: 05 Aug 2025
-- Description:	Load transaction records
-- ==========================================================
-- 04 Aug 2025 - Dave Brown - ContactId filter added
-- 21 Aug 2025 - Dave Brown - When AccountId = -1
--							  return both Owner’s Capital/Investment and Owner’s Drawings
-- ==========================================================
CREATE PROCEDURE [finance].[TransactionDetail_LoadFiltered]
	@FromDate			datetime = null,
	@ToDate				datetime = null,
	@AccountId			int = null,
	@ContactId			int = null,
	@TransactionTypeId	int = null,
	@PageSize			INT = 20,
	@CurrentPage		INT = 1,
	@TotalPages			INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Err int;
    DECLARE @TotalCount int;

    -- Calculate total count
    SELECT @TotalCount = COUNT(1)
    FROM	finance.TransactionDetail td
	INNER JOIN	finance.[Transaction] t ON td.TransactionId = t.Id
    WHERE	(@FromDate IS NULL OR td.[Date] >= @FromDate)
		AND (@ToDate IS NULL OR td.[Date] < DATEADD(DAY, 1, @ToDate))
		AND (ISNULL(@AccountId, 0) = 0 OR td.AccountId = @AccountId 
			OR 
			(@AccountId = -1 AND td.AccountId in (3,4))) -- Owner’s Capital/Investment and Owner’s Drawings
		AND (ISNULL(@ContactId, 0) = 0 OR td.ContactId = @ContactId)
		AND (ISNULL(@TransactionTypeId, 0) = 0 OR t.TransactionTypeId = @TransactionTypeId)
		AND td.Deleted = 0

    -- Calculate total pages
    SELECT @TotalPages = 
        CASE 
            WHEN @TotalCount = 0 THEN 1
            ELSE CEILING(CAST(@TotalCount AS float) / @PageSize)
        END;


	SELECT	td.Id,
			td.TransactionId,
			t.TransactionTypeId,
			tt.[Type],
			td.[Date], 
			t.Reference,
			td.AccountId, 
			a.[Name] AS Account, 
			td.[Description],
			td.Amount,
			td.Direction,
			td.ContactId,
			COALESCE(c.[Name], '') AS ContactName,
			CASE WHEN Direction = 1 THEN Amount ELSE 0 END AS Debit, 
			CASE WHEN Direction = -1 THEN Amount ELSE 0 END AS Credit
	FROM	finance.TransactionDetail td
	INNER JOIN	finance.[Transaction] t ON td.TransactionId = t.Id
	INNER JOIN	finance.[TransactionType] tt ON t.TransactionTypeId = tt.Id
	INNER JOIN	finance.Account a ON td.AccountId = a.Id
	LEFT OUTER JOIN dbo.Contact c on td.ContactId = c.Id
	WHERE	(@FromDate IS NULL OR td.[Date] >= @FromDate)
		AND (@ToDate IS NULL OR td.[Date] < DATEADD(DAY, 1, @ToDate))
		AND (ISNULL(@AccountId, 0) = 0 OR td.AccountId = @AccountId 
			OR 
			(@AccountId = -1 AND td.AccountId in (3,4))) -- Owner’s Capital/Investment and Owner’s Drawings
		AND (ISNULL(@ContactId, 0) = 0 OR td.ContactId = @ContactId)
		AND (ISNULL(@TransactionTypeId, 0) = 0 OR t.TransactionTypeId = @TransactionTypeId)
		AND td.Deleted = 0
	ORDER BY	td.[Date] DESC, td.TransactionId, a.[Name], Credit, Debit
    OFFSET (@CurrentPage - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

	RETURN 0

END
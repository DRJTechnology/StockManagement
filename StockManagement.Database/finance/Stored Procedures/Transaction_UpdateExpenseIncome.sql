-- ==========================================================================
-- Author:		Dave Brown
-- Create date: 03 Aug 2025
-- Description:	Updates an expense transaction and transaction detail records
-- ==========================================================================
-- 04 Aug 2025 - Updated Expense procedure to handle Income too
-- ==========================================================================
CREATE PROCEDURE [finance].[Transaction_UpdateExpenseIncome]
	@Success			BIT OUTPUT,
	@TransactionTypeId	INT, -- Expense = 2, Income = 3
	@Id					INT, 
	@AccountId			INT,
	@Date				DATETIME,
	@Description		NVARCHAR(512) = NULL,
	@Amount				MONEY,
	@ContactId			INT,
	@CurrentUserId		INT
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	DECLARE @TransactionId			INT,
			@AssociatedAccountId	INT = 3 -- Owner’s Investment/Drawings account
	
	SELECT	@TransactionId = td.TransactionId
	FROM	finance.[TransactionDetail] td
	INNER JOIN finance.[Transaction] t ON td.TransactionId = t.Id
	Where	td.Id = @Id
		AND t.TransactionTypeId = @TransactionTypeId
		AND td.Deleted = 0

	IF (@TransactionId IS NOT NULL)
	BEGIN
		UPDATE	[finance].[Transaction]
		SET		[Date] = @Date,
				AmendUserId = @CurrentUserId,
				AmendDate = SYSDATETIME()
		WHERE	Id = @TransactionId
			AND	Deleted = 0

		UPDATE	[finance].[TransactionDetail]
		SET		AccountId = @AccountId,
				[Date] = @Date,
				[Description] = @Description,
				Amount = @Amount,
				ContactId = @ContactId,
				AmendUserId = @CurrentUserId,
				AmendDate = SYSDATETIME()
		WHERE	Id = @Id
			AND	Deleted = 0

		UPDATE	[finance].[TransactionDetail]
		SET		[Date] = @Date,
				[Description] = @Description,
				Amount = @Amount,
				ContactId = @ContactId,
				AmendUserId = @CurrentUserId,
				AmendDate = SYSDATETIME()
		WHERE	TransactionId = @TransactionId
			AND	AccountId = @AssociatedAccountId
			AND	Deleted = 0

		SET @Success = 1
	END
	ELSE
	BEGIN
		SET @Success = 0
	END

	SET @Err = @@Error

	RETURN @Err
END

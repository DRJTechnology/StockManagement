-- =========================================================================
-- Author:		Dave Brown
-- Create date: 30 Jul 2025
-- Description:	Creates a expense transactiona nd transaction detail records
-- =========================================================================
-- 04 Aug 2025 - Updated Expense procedure to handle Income too
-- =========================================================================
CREATE PROCEDURE [finance].[Transaction_CreateExpenseIncome]
	@Success			BIT OUTPUT,
	@Id					INT OUTPUT, 
	@TransactionTypeId	INT, -- Expense = 2, Income = 3
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
			@Direction				SMALLINT,
			@AssociatedAccountId	INT = 3, -- Owner’s Investment/Drawings account
			@ContactName			NVARCHAR(100),
			@ReferencePrefix		NVARCHAR(3)

	SELECT	@ContactName = [Name]
	FROM	dbo.Contact
	WHERE	Id = @ContactId
	
	IF (@TransactionTypeId = 2) -- Expense
	BEGIN
		SET @Direction = 1 -- Debit for Expense
		SET @ReferencePrefix = 'EXP' -- Prefix for Expense transactions
	END
	ELSE IF (@TransactionTypeId = 3) -- Income
	BEGIN
		SET @Direction = -1 -- Credit for Income
		SET @ReferencePrefix = 'INC' -- Prefix for Income transactions
	END

	DECLARE @Reference NVARCHAR(256) = @ReferencePrefix + '-' + CONVERT(NVARCHAR(20), @Date, 112) + @ContactName

    INSERT INTO [finance].[Transaction] (TransactionTypeId, [Date], Reference, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
	VALUES (@TransactionTypeId, @Date, @Reference, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME())

	SELECT @TransactionId = SCOPE_IDENTITY()

    INSERT INTO [finance].[TransactionDetail] (TransactionId, AccountId, [Date], [Description], Amount, Direction, ContactId, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
	VALUES (@TransactionId, @AccountId, @Date, @Description, @Amount, @Direction, @ContactId, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME())
	SELECT @Id = SCOPE_IDENTITY()

    INSERT INTO [finance].[TransactionDetail] (TransactionId, AccountId, [Date], [Description], Amount, Direction, ContactId, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
	VALUES (@TransactionId, @AssociatedAccountId, @Date, @Description, @Amount, @Direction * -1, null, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME())

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

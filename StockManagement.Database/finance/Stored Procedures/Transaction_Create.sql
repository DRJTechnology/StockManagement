-- =========================================================================
-- Author:		Dave Brown
-- Create date: 02 Sep 2025
-- Description:	Creates a transaction and transaction detail records
-- =========================================================================
CREATE PROCEDURE [finance].[Transaction_Create]
	@Success			BIT OUTPUT,
	@Id					INT OUTPUT, 
	@TransactionTypeId	INT, -- Journal = 1, Expense = 2, Income = 3, Sale = 4
	@DebitAccountId		INT,
	@CreditAccountId	INT,
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

	DECLARE @TransactionId				INT,
			@ContactName				NVARCHAR(100),
			@ReferencePrefix			NVARCHAR(3)
	
	IF (@TransactionTypeId = 1) -- Journal
	BEGIN
		SET @ReferencePrefix = 'JNL' -- Prefix for Journal transactions
	END
	ELSE IF (@TransactionTypeId = 2) -- Expense
	BEGIN
		SET @ReferencePrefix = 'EXP' -- Prefix for Expense transactions
	END
	ELSE IF (@TransactionTypeId = 3) -- Income
	BEGIN
		SET @ReferencePrefix = 'INC' -- Prefix for Income transactions
	END
	ELSE IF (@TransactionTypeId = 4) -- Sale
	BEGIN
		SET @ReferencePrefix = 'SAL' -- Prefix for Sale transactions
	END

	DECLARE @Reference NVARCHAR(256) = @ReferencePrefix + '-' + CONVERT(NVARCHAR(20), @Date, 112) + '-' + CONVERT(VARCHAR(10), ISNULL(@ContactId,0))

    INSERT INTO [finance].[Transaction] (TransactionTypeId, [Date], Reference, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
	VALUES (@TransactionTypeId, @Date, @Reference, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME())

	SELECT @TransactionId = SCOPE_IDENTITY()

	-- Debit Account
    INSERT INTO [finance].[TransactionDetail] (TransactionId, AccountId, [Date], [Description], Amount, Direction, ContactId, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
	VALUES (@TransactionId, @DebitAccountId, @Date, @Description, @Amount, 1 /* Direction */, @ContactId, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME())
	SELECT @Id = SCOPE_IDENTITY()

	-- Credit Account
    INSERT INTO [finance].[TransactionDetail] (TransactionId, AccountId, [Date], [Description], Amount, Direction, ContactId, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
	VALUES (@TransactionId, @CreditAccountId, @Date, @Description, @Amount, -1 /* Direction */, @ContactId, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME())

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

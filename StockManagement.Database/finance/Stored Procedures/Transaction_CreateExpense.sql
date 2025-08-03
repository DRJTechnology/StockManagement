-- =========================================================================
-- Author:		Dave Brown
-- Create date: 30 Jul 2025
-- Description:	Creates a expense transactiona nd transaction detail records
-- =========================================================================
CREATE PROCEDURE [finance].[Transaction_CreateExpense]
	@Success			BIT OUTPUT,
	@Id					INT OUTPUT, 
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
			@TransactionTypeId		INT = 2, -- Expense
			@Direction				SMALLINT = 1,
			@AssociatedAccountId	INT = 3, -- Owner’s Investment/Drawings account
			@SupplierName			NVARCHAR(100)

	SELECT	@SupplierName = [Name]
	FROM	dbo.Contact
	WHERE	Id = @ContactId
	
	DECLARE @Reference NVARCHAR(256) = 'EXP-' + '-' + CONVERT(NVARCHAR(20), @Date, 112) + @SupplierName

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

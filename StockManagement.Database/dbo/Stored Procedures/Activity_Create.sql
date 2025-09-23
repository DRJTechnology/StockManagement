-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Activity
-- =========================================================
-- 30 Aug 2025 - Dave Brown - Added Notes
-- 01 Sep 2025 - Dave Brown - InventoryBatch records updated
-- 02 Sep 2025 - Dave Brown - Transactions added
-- 23 Sep 2025 - Dave Brown - Updated @Id to @@TransactionDetailId for transaction create
-- =========================================================
CREATE PROCEDURE [dbo].[Activity_Create]
(
	@Success bit output,
	@Id int output,
	@ActivityDate datetime,
	@ActionId int,
	@ProductId int,
	@ProductTypeId int,
	@LocationId int,
	@Quantity int,
	@Notes nvarchar(1024) = NULL,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	SET XACT_ABORT ON;

	DECLARE @Err int = 0;
	DECLARE @UpdateDate DATETIME = GETDATE();
	DECLARE @CostRemoved MONEY = 0;
	DECLARE @TransactionDetailId INT;

	-- Initialize output parameters
	SET @Success = 0;
	SET @Id = NULL;

	BEGIN TRANSACTION;

	BEGIN TRY
		-- Insert the activity record
		INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[LocationId],[Quantity],[Notes],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@ActivityDate, @ActionId,@ProductId, @ProductTypeId,@LocationId, @Quantity, @Notes, @Deleted, @CurrentUserId, @UpdateDate)

		SELECT @Id = SCOPE_IDENTITY()

		-- Handle inventory batch movements based on action
		IF @ActionId = 2 -- Move from stock room to
		BEGIN
			EXEC	[finance].[InventoryBatch_MoveQuantity]
					@ProductId = @ProductId,
					@ProductTypeId = @ProductTypeId,
					@FromLocationId = 1, -- Stock room
					@ToLocationId = @LocationId,
					@Quantity = @Quantity,
					@ActivityId = @Id,
					@CurrentUserId = @CurrentUserId
		END
		ELSE IF @ActionId = 3 -- Move to stock room from
		BEGIN
			EXEC	[finance].[InventoryBatch_MoveQuantity]
					@ProductId = @ProductId,
					@ProductTypeId = @ProductTypeId,
					@FromLocationId = @LocationId,
					@ToLocationId = 1, -- Stock room
					@Quantity = @Quantity,
					@ActivityId = @Id,
					@CurrentUserId = @CurrentUserId
		END
		ELSE IF @ActionId = 4 -- Delete Stock
		BEGIN
		    EXEC	[finance].[InventoryBatch_ReduceQuantity]
					@ProductId = @ProductId,
					@ProductTypeId = @ProductTypeId,
					@LocationId = @LocationId,
					@Quantity = @Quantity,
					@ActivityId = @Id,
					@CurrentUserId = @CurrentUserId,
					@CostRemoved = @CostRemoved OUTPUT

			EXEC [finance].[Transaction_Create]
				@Success			OUTPUT,
				@TransactionDetailId OUTPUT, 
				@TransactionTypeId	= 1, -- Journal = 1, Expense = 2, Income = 3, Sale = 4
				@DebitAccountId		= 10,-- Inventory Shrinkage (Expense)
				@CreditAccountId	= 6, -- Inventory
				@Date				= @ActivityDate,
				@Description		= 'Deleted stock',
				@Amount				= @CostRemoved,
				@ContactId			= NULL,
				@CurrentUserId		= @CurrentUserId

		END
		ELSE IF @ActionId = 6 -- Promotional use
		BEGIN
		    EXEC	[finance].[InventoryBatch_ReduceQuantity]
					@ProductId = @ProductId,
					@ProductTypeId = @ProductTypeId,
					@LocationId = @LocationId,
					@Quantity = @Quantity,
					@ActivityId = @Id,
					@CurrentUserId = 1,
					@CostRemoved = @CostRemoved OUTPUT

			EXEC [finance].[Transaction_Create]
				@Success			OUTPUT,
				@TransactionDetailId OUTPUT, 
				@TransactionTypeId	= 3, -- Journal = 1, Expense = 2, Income = 3, Sale = 4
				@DebitAccountId		= 8,-- Advertising/Promotion (Expense)
				@CreditAccountId	= 6, -- Inventory
				@Date				= @ActivityDate,
				@Description		= 'Stock for promotional use',
				@Amount				= @CostRemoved,
				@ContactId			= NULL,
				@CurrentUserId		= @CurrentUserId

		END
		ELSE IF @ActionId = 7 -- Damaged stock
		BEGIN
		    EXEC	[finance].[InventoryBatch_ReduceQuantity]
					@ProductId = @ProductId,
					@ProductTypeId = @ProductTypeId,
					@LocationId = @LocationId,
					@Quantity = @Quantity,
					@ActivityId = @Id,
					@CurrentUserId = 1,
					@CostRemoved = @CostRemoved OUTPUT

			EXEC [finance].[Transaction_Create]
				@Success			OUTPUT,
				@TransactionDetailId OUTPUT, 
				@TransactionTypeId	= 1, -- Journal = 1, Expense = 2, Income = 3, Sale = 4
				@DebitAccountId		= 10,-- Inventory Shrinkage (Expense)
				@CreditAccountId	= 6, -- Inventory
				@Date				= @ActivityDate,
				@Description		= 'Damaged stock',
				@Amount				= @CostRemoved,
				@ContactId			= NULL,
				@CurrentUserId		= @CurrentUserId

		END
		ELSE IF @ActionId = 8 -- Personal use
		BEGIN
		    EXEC	[finance].[InventoryBatch_ReduceQuantity]
					@ProductId = @ProductId,
					@ProductTypeId = @ProductTypeId,
					@LocationId = @LocationId,
					@Quantity = @Quantity,
					@ActivityId = @Id,
					@CurrentUserId = 1,
					@CostRemoved = @CostRemoved OUTPUT

			EXEC [finance].[Transaction_Create]
				@Success			OUTPUT,
				@TransactionDetailId OUTPUT, 
				@TransactionTypeId	= 1, -- Journal = 1, Expense = 2, Income = 3, Sale = 4
				@DebitAccountId		= 4 ,-- Owner’s Drawings (Equity)
				@CreditAccountId	= 6, -- Inventory
				@Date				= @ActivityDate,
				@Description		= 'Stock for owner''s personal use',
				@Amount				= @CostRemoved,
				@ContactId			= NULL,
				@CurrentUserId		= @CurrentUserId

		END

		-- If we get here, all operations succeeded
		SET @Success = 1;
		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		-- Rollback the transaction
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
		END
		
		INSERT INTO dbo.ErrorLog
		(ErrorDate,	ProcedureName, ErrorNumber, ErrorSeverity, ErrorState, ErrorLine, ErrorMessage, UserId)
		VALUES (GETDATE(), ERROR_PROCEDURE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_MESSAGE(), @CurrentUserId);

		-- Capture error information
		SET @Err = ERROR_NUMBER();
		SET @Success = 0;
		SET @Id = NULL;

		-- Re-raise the error
		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
		DECLARE @ErrorState INT = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH

	RETURN @Err;
END
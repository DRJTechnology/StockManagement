-- =========================================================
-- Author:		Dave Brown
-- Create date: 13 Sep 2025
-- Description:	Confirm a stock sale
-- =========================================================
CREATE PROCEDURE [dbo].[StockSale_ConfirmSale]
(
	@Success bit output,
	@StockSaleId int,
	@FromLocationId int,
	@ContactId int, -- Customer
	@TotalPrice money,
	@StockSaleDetails finance.StockSaleConfirmTableType READONLY,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME,
			@SaleDate DATETIME,
			@TransactionDetailId INT,
			@TranstionSuccess BIT,
			@ActivityId INT,
			@CostRemoved MONEY
	SET		@UpdateDate = GetDate()

	SELECT	@SaleDate = [Date]
	FROM	dbo.StockSale
	WHERE	Id = @StockSaleId

	BEGIN TRY
		BEGIN TRANSACTION

		UPDATE	dbo.StockSale
		SET		TotalPrice = @TotalPrice,
				SaleConfirmed = 1,
				AmendUserID = @CurrentUserId,
				AmendDate = @UpdateDate
		WHERE	Id = @StockSaleId

		-- Create a cursor to iterate through the @StockSaleDetails table variable
		DECLARE @ProductId INT,
				@ProductTypeId INT,
				@Quantity INT,
				@UnitPrice MONEY,
				@StockSaleDetailId INT,
				@InventoryBatchId INT
	
		DECLARE StockSaleCursor CURSOR FOR
		SELECT	ProductId, ProductTypeId, Quantity, UnitPrice, StockSaleDetailId
		FROM	@StockSaleDetails

		OPEN StockSaleCursor
		FETCH NEXT FROM StockSaleCursor INTO @ProductId, @ProductTypeId, @Quantity, @UnitPrice, @StockSaleDetailId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			SELECT	@ActivityId = Id
			FROM	dbo.Activity
			WHERE	ActionId = 5 -- Sale from
				AND StockSaleDetailId = @StockSaleDetailId

			-- Remove Stock
			EXEC	[finance].[InventoryBatch_ReduceQuantity]
						@ProductId = @ProductId,
						@ProductTypeId = @ProductTypeId,
						@LocationId = @FromLocationId,
						@Quantity = @Quantity,
						@ActivityId = @ActivityId,
						@CurrentUserId = @CurrentUserId,
						@CostRemoved = @CostRemoved OUTPUT

			-- Create Transaction for Cost of Goods Sold
			EXEC	[finance].[Transaction_Create]
						@Success = @TranstionSuccess OUTPUT,
						@TransactionDetailId = @TransactionDetailId OUTPUT,
						@TransactionTypeId = 4, -- Sale
						@DebitAccountId = 9, -- Cost of Goods Sold
						@CreditAccountId = 6, -- Inventory
						@Date = @SaleDate,
						@Description = N'Sale of stock',
						@Amount = @CostRemoved,
						@ContactId = @ContactId,
						@CurrentUserId = @CurrentUserId

			-- If transaction creation failed, raise an error to trigger the CATCH block
			IF @TranstionSuccess = 0
			BEGIN
				RAISERROR('Transaction creation failed.', 16, 1)
			END
			
			-- Link transaction to stock sale detail
			UPDATE	dbo.StockSaleDetail
			SET		UnitPrice = @UnitPrice,
					TransactionDetailId = @TransactionDetailId,
					AmendUserID = @CurrentUserId,
					AmendDate = @UpdateDate
			WHERE	Id = @StockSaleDetailId

			FETCH NEXT FROM StockSaleCursor INTO @ProductId, @ProductTypeId, @Quantity, @UnitPrice, @StockSaleDetailId
		END
		CLOSE StockSaleCursor
		DEALLOCATE StockSaleCursor

		COMMIT TRANSACTION

		SET @Success = 1
		SET @Err = 0
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		
		INSERT INTO dbo.ErrorLog (ErrorDate,	ProcedureName, ErrorNumber, ErrorSeverity, ErrorState, ErrorLine, ErrorMessage, UserId)
		VALUES (GETDATE(), ERROR_PROCEDURE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_MESSAGE(), @CurrentUserId);

		SET @Success = 0
		SET @Err = ERROR_NUMBER()
	END CATCH

	RETURN @Err
END
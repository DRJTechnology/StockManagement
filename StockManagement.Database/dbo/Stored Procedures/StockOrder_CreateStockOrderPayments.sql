-- =========================================================
-- Author:		Dave Brown
-- Create date: 28 Aug 2025
-- Description:	Create Stock order payments
-- =========================================================
CREATE PROCEDURE [dbo].[StockOrder_CreateStockOrderPayments]
(
	@Success bit output,
	@StockOrderId int,
	@ContactId int,
	@Cost money,
	@Description NVARCHAR(512),
	@PaymentDate datetime,
	@StockPaymentDetails finance.StockPaymentTableType READONLY,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME,
			@TransactionId INT,
			@TransactionDetailId INT,
			@TranstionSuccess BIT
	SET		@UpdateDate = GetDate()

	BEGIN TRY
		BEGIN TRANSACTION

		-- Create the transaction for the payment
		EXEC [finance].[Transaction_CreateExpenseIncome] 
						@Success = @TranstionSuccess OUTPUT,
						@Id = @TransactionDetailId OUTPUT,
						@TransactionId = @TransactionId OUTPUT,
						@TransactionTypeId = 2, -- Expense
						@AccountId = 6, -- Inventory
						@Date = @PaymentDate,
						@Description = @Description,
						@Amount = @Cost,
						@ContactId = @ContactId,
						@CurrentUserId = @CurrentUserId

		-- Link the transaction to the stock order
		UPDATE	dbo.StockOrder
		SET		TransactionId = @TransactionId,
				PaymentRecorded = 1,
				AmendUserID = @CurrentUserId,
				AmendDate = @UpdateDate
		WHERE Id = @StockOrderId

		-- Create a cursor to iterate through the @StockPaymentDetails table variable
		DECLARE @ProductId INT,
				@ProductTypeId INT,
				@Quantity INT,
				@UnitPrice MONEY,
				@StockOrderDetailId INT,
				@InventoryBatchId INT
	
		DECLARE StockPaymentCursor CURSOR FOR
		SELECT ProductId, ProductTypeId, Quantity, UnitPrice, StockOrderDetailId
		FROM @StockPaymentDetails

		OPEN StockPaymentCursor
		FETCH NEXT FROM StockPaymentCursor INTO @ProductId, @ProductTypeId, @Quantity, @UnitPrice, @StockOrderDetailId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			INSERT INTO finance.InventoryBatch (InventoryBatchStatusId, ProductId, ProductTypeId, LocationId, InitialQuantity, QuantityRemaining, UnitCost, PurchaseDate, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
			SELECT	1 /* pending */, ProductId, ProductTypeId, 1 /* stockroom */, Quantity, Quantity, UnitPrice, @UpdateDate, 0, @CurrentUserId, @UpdateDate, @CurrentUserId, @UpdateDate
			FROM	@StockPaymentDetails
			WHERE	StockOrderDetailId = @StockOrderDetailId

			SET @InventoryBatchId = SCOPE_IDENTITY()

			-- Insert into InventoryBatchActivity
			INSERT INTO finance.InventoryBatchActivity (InventoryBatchId, ActivityId, Quantity, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
			SELECT @InventoryBatchId, Id, @Quantity, 0, @CurrentUserId, @UpdateDate, @CurrentUserId, @UpdateDate
			FROM dbo.Activity
			WHERE	ActionId = 1 -- Add new stock add to
				AND StockOrderDetailId = @StockOrderDetailId

			UPDATE	dbo.StockOrderDetail
			SET		InitialInventoryBatchId = @InventoryBatchId,
					AmendUserID = @CurrentUserId,
					AmendDate = @UpdateDate
			WHERE	Id = @StockOrderDetailId

			FETCH NEXT FROM StockPaymentCursor INTO @ProductId, @ProductTypeId, @Quantity, @UnitPrice, @StockOrderDetailId
		END
		CLOSE StockPaymentCursor
		DEALLOCATE StockPaymentCursor

		COMMIT TRANSACTION

		SET @Success = 1
		SET @Err = 0
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		
		INSERT INTO dbo.ErrorLog
		(ErrorDate,	ProcedureName, ErrorNumber, ErrorSeverity, ErrorState, ErrorLine, ErrorMessage, UserId)
		VALUES (GETDATE(), ERROR_PROCEDURE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_MESSAGE(), @CurrentUserId);

		SET @Success = 0
		SET @Err = ERROR_NUMBER()
	END CATCH

	RETURN @Err
END
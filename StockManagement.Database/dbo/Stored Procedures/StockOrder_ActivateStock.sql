-- ==================================================================
-- Author:		Dave Brown
-- Create date: 29 Aug 2025
-- Description:	Activate pending inventory bactches for a stock order
-- ==================================================================
CREATE PROCEDURE [dbo].[StockOrder_ActivateStock]
(
	@Success bit output,
	@StockOrderId int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET		@UpdateDate = GetDate()

	BEGIN TRY
		BEGIN TRANSACTION

		-- Update any pending inventory batches to active for the stock order
		UPDATE ib
		SET InventoryBatchStatusId = 2, -- Active
			AmendUserId = @CurrentUserId,
			AmendDate = @UpdateDate
		FROM	finance.InventoryBatch ib
		INNER JOIN dbo.StockOrderDetail sod ON ib.Id = sod.InitialInventoryBatchId
		WHERE	sod.StockOrderId = @StockOrderId
			AND ib.InventoryBatchStatusId = 1 -- Pending

		-- Update the stock order StockReceiptRecorded
		UPDATE dbo.StockOrder
		SET StockReceiptRecorded = 1,
			AmendUserID = @CurrentUserId,
			AmendDate = @UpdateDate
		WHERE Id = @StockOrderId

		COMMIT TRANSACTION

		SET @Success = 1
		SET @Err = 0
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION

		SET @Success = 0
		SET @Err = ERROR_NUMBER()
	END CATCH

	RETURN @Err
END
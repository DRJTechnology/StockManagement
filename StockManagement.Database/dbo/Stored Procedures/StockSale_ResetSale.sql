-- =========================================================
-- Author:		Dave Brown
-- Create date: 22 Sep 2025
-- Description:	Reset a sale
-- =========================================================
CREATE PROCEDURE [dbo].[StockSale_ResetSale]
(
	@Success bit output,
	@StockSaleId int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE	@AmendDate		DATETIME = GETDATE(),
			@SaleConfirmed	BIT,
			@PaymentReceived BIT

	SELECT	@SaleConfirmed = SaleConfirmed,
			@PaymentReceived = PaymentReceived
	FROM	dbo.StockSale
	WHERE	Id = @StockSaleId

	BEGIN TRY
		BEGIN TRANSACTION

		IF (@PaymentReceived = 1)
		BEGIN
			-- Delete the Transaction and TransactionDetails for the "Owner’s Drawings/Sales - Art" transaction.
			UPDATE	t
			SET		t.Deleted = 1,
					t.AmendUserID = @CurrentUserId,
					t.AmendDate = @AmendDate
			FROM	finance.[Transaction] t
			INNER JOIN dbo.StockSale ss ON t.Id = ss.TransactionId
			WHERE	ss.Id = @StockSaleId
				AND	t.Deleted = 0

			UPDATE	td
			SET		td.Deleted = 1,
					td.AmendUserID = @CurrentUserId,
					td.AmendDate = @AmendDate
			FROM	finance.TransactionDetail td
			INNER JOIN dbo.StockSale ss ON td.TransactionId = ss.TransactionId
			WHERE	ss.Id = @StockSaleId
				AND	td.Deleted = 0

			-- Clear the TransationId and PaymentReceived flag from StockSale
			UPDATE	dbo.StockSale
			SET		TransactionId = NULL,
					PaymentReceived = 0,
					AmendUserID = @CurrentUserId,
					AmendDate = @AmendDate
			WHERE	Id = @StockSaleId
		END

		IF (@SaleConfirmed = 1)
		BEGIN

			-- For each Detail record
				-- Increase the InventoryBatch RemainingQuantity
				WITH BatchSums AS (
					SELECT	iba.InventoryBatchId, SUM(iba.Quantity) AS TotalQuantity
					FROM	finance.InventoryBatchActivity iba
					INNER JOIN dbo.Activity a ON iba.ActivityId = a.Id
					INNER JOIN StockSaleDetail ssd ON a.StockSaleDetailId = ssd.Id
					WHERE	ssd.StockSaleId = @StockSaleId
					GROUP BY iba.InventoryBatchId
				)
				UPDATE	ib
				SET		QuantityRemaining = QuantityRemaining + bs.TotalQuantity,
						InventoryBatchStatusId = 2, -- Active
						Deleted = 0,
						AmendUserId = @CurrentUserId,
						AmendDate = @AmendDate
				FROM	finance.InventoryBatch ib
				INNER JOIN BatchSums bs ON ib.Id = bs.InventoryBatchId

				-- Delete the COGS transaction details
				UPDATE	td_2
				SET		td_2.Deleted = 1,
						td_2.AmendUserID = @CurrentUserId,
						td_2.AmendDate = @AmendDate
				FROM	finance.[TransactionDetail] td_1
				INNER JOIN	finance.[Transaction] t ON td_1.TransactionId = t.Id
				INNER JOIN	finance.[TransactionDetail] td_2 ON t.Id = td_2.TransactionId
				INNER JOIN	StockSaleDetail ssd ON td_1.Id = ssd.TransactionDetailId
				WHERE	ssd.StockSaleId = @StockSaleId
					AND td_2.Deleted = 0

				-- Delete the COGS transaction
				UPDATE	t
				SET		t.Deleted = 1,
						t.AmendUserID = @CurrentUserId,
						t.AmendDate = @AmendDate
				FROM	finance.[Transaction] t
				INNER JOIN	finance.[TransactionDetail] td ON t.Id = td.TransactionId
				INNER JOIN	StockSaleDetail ssd ON td.Id = ssd.TransactionDetailId
				WHERE	ssd.StockSaleId = @StockSaleId
					AND t.Deleted = 0

				-- Clear the UnitPrice and the TransactionDetailId from the StockSaleDetail record
				UPDATE	dbo.StockSaleDetail
				SET		UnitPrice = 0,
						TransactionDetailId = NULL,
						AmendUserId = @CurrentUserId,
						AmendDate = @AmendDate
				WHERE	StockSaleId = @StockSaleId
					AND Deleted = 0

				
				-- Delete the InventoryBatchActvity records
				UPDATE	iba
				SET		iba.Deleted = 1,
						iba.AmendUserID = @CurrentUserId,
						iba.AmendDate = @AmendDate
				FROM	finance.InventoryBatchActivity iba
				INNER JOIN dbo.Activity a ON iba.ActivityId = a.Id
				INNER JOIN StockSaleDetail ssd ON a.StockSaleDetailId = ssd.Id
				WHERE	ssd.StockSaleId = @StockSaleId


			-- Clear the total price and SaleConfirmed flag from StockSale
			UPDATE	dbo.StockSale
			SET		TotalPrice = 0,
					SaleConfirmed = 0,
					AmendUserID = @CurrentUserId,
					AmendDate = @AmendDate
			WHERE	Id = @StockSaleId

		END

		-- Add the deleted inventory items back in


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
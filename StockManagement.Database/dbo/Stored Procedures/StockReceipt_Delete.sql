-- =========================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Delete Stock Receipt
-- =========================================================
CREATE PROCEDURE [dbo].[StockReceipt_Delete]
(
	@Success bit output,
	@StockReceiptId int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	DECLARE @UpdateDate		DATETIME
	SET @UpdateDate = GetDate()

	BEGIN TRY
		BEGIN TRANSACTION

		-- Soft delete the stock receipt
		UPDATE [StockReceipt]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE
			[Id] = @StockReceiptId

		-- Soft delete the stock receipt details
		UPDATE [StockReceiptDetail]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE	[StockReceiptId] = @StockReceiptId
			AND [Deleted] = 0

		-- Soft delete the activities associated with the stock receipt details
		UPDATE a
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		FROM
			[Activity] a
			INNER JOIN [StockReceiptDetail] srd ON a.[StockReceiptDetailId] = srd.[Id]
		WHERE	srd.[StockReceiptId] = @StockReceiptId
			AND a.[Deleted] = 0

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
-- =========================================================
-- Author:		Dave Brown
-- Create date: 03 Jul 2025
-- Description:	Delete Delivery Note
-- =========================================================
CREATE PROCEDURE [dbo].[StockSale_Delete]
(
	@Success bit output,
	@StockSaleId int,
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

		-- Soft delete the delivery note
		UPDATE [StockSale]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE
			[Id] = @StockSaleId

		-- Soft delete the delivery note details
		UPDATE [StockSaleDetail]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE	[StockSaleId] = @StockSaleId
			AND [Deleted] = 0

		-- Soft delete the activities associated with the delivery note details
		UPDATE a
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		FROM
			[Activity] a
			INNER JOIN [StockSaleDetail] dnd ON a.[StockSaleDetailId] = dnd.[Id]
		WHERE	dnd.[StockSaleId] = @StockSaleId
			AND a.[Deleted] = 0

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
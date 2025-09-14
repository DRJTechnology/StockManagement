-- =========================================================
-- Author:		Dave Brown
-- Create date: 03 Jul 2025
-- Description:	Delete Delivery Note Detail
-- =========================================================
CREATE PROCEDURE [dbo].[StockSaleDetail_Delete]
(
	@Success bit output,
	@StockSaleDetailId int,
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

		UPDATE [StockSaleDetail]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE
			[Id] = @StockSaleDetailId

		UPDATE [Activity]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE	[StockSaleDetailId] = @StockSaleDetailId
			AND Deleted = 0

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
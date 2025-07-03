-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete Delivery Note Detail
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNoteDetail_Delete]
(
	@Success bit output,
	@DeliveryNoteDetailId int,
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

		UPDATE [DeliveryNoteDetail]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE
			[Id] = @DeliveryNoteDetailId

		UPDATE [Activity]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE
			[DeliveryNoteDetailId] = @DeliveryNoteDetailId

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
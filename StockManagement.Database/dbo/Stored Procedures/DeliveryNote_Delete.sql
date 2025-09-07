-- =========================================================
-- Author:		Dave Brown
-- Create date: 03 Jul 2025
-- Description:	Delete Delivery Note
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNote_Delete]
(
	@Success bit output,
	@DeliveryNoteId int,
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
		UPDATE [DeliveryNote]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE
			[Id] = @DeliveryNoteId

		-- Soft delete the delivery note details
		UPDATE [DeliveryNoteDetail]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE	[DeliveryNoteId] = @DeliveryNoteId
			AND [Deleted] = 0

		-- Soft delete the activities associated with the delivery note details
		UPDATE a
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		FROM
			[Activity] a
			INNER JOIN [DeliveryNoteDetail] dnd ON a.[DeliveryNoteDetailId] = dnd.[Id]
		WHERE	dnd.[DeliveryNoteId] = @DeliveryNoteId
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
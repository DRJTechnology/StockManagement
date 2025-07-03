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

	UPDATE [DeliveryNoteDetail]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @DeliveryNoteDetailId

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
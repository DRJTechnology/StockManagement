-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete Location
-- =========================================================
CREATE PROCEDURE [dbo].[Location_Delete]
(
	@Success bit output,
	@LocationID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Location]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @LocationID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
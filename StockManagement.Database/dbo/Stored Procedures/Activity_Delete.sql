-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete Activity
-- =========================================================
CREATE PROCEDURE [dbo].[Activity_Delete]
(
	@Success bit output,
	@ActivityID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Activity]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @ActivityID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
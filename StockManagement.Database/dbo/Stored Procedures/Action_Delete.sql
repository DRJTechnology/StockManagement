-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete Action
-- =========================================================
CREATE PROCEDURE [dbo].[Action_Delete]
(
	@Success bit output,
	@ActionID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Action]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @ActionID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
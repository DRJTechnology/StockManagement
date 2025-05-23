-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update Action
-- =========================================================
CREATE PROCEDURE [dbo].[Action_Update]
(
	@Success bit output,
	@Id int,
	@ActionName nvarchar(50),
	@Direction int,
	@AffectStockRoom bit,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Action]
	SET
		[ActionName] = @ActionName,
		[Direction] = @Direction,
		[AffectStockRoom] = @AffectStockRoom,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
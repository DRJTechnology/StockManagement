-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update Location
-- =========================================================
CREATE PROCEDURE [dbo].[Location_Update]
(
	@Success bit output,
	@Id int,
	@Name nvarchar(50),
	@Notes nvarchar(4000),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Location]
	SET
		[Name] = @Name,
		[Notes] = @Notes,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
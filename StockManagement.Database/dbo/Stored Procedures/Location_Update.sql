-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update Location
-- =========================================================
-- 10 Sep 2025 - DB Updated to include ContactId
-- =========================================================
CREATE PROCEDURE [dbo].[Location_Update]
(
	@Success bit output,
	@Id int,
	@Name nvarchar(50),
	@Notes nvarchar(4000),
	@ContactId int,
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
		[ContactId] = @ContactId,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
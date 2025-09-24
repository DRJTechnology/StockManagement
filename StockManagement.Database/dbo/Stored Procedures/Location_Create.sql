-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Location
-- =========================================================
-- 11 Jun 2025 - DB Updated to include Notes field
-- 10 Sep 2025 - DB Updated to include ContactId
-- =========================================================
CREATE PROCEDURE [dbo].[Location_Create]
(
	@Success bit output,
	@Id int output,
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

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Location] ([Name],[Notes],[ContactId],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@Name, @Notes, @ContactId, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @Id = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
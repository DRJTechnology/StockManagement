-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Venue
-- =========================================================
-- 11 Jun 2025 - DB Updated to include Notes field
CREATE PROCEDURE [dbo].[Venue_Create]
(
	@Success bit output,
	@Id int output,
	@VenueName nvarchar(50),
	@Notes nvarchar(4000),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Venue] ([VenueName],[Notes],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@VenueName, @Notes, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Venue
-- =========================================================
CREATE PROCEDURE [dbo].[Venue_Create]
(
	@Success bit output,
	@Id int output,
	@VenueName nvarchar(50),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Venue] ([VenueName],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@VenueName, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
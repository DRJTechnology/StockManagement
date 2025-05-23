-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update Venue
-- =========================================================
CREATE PROCEDURE [dbo].[Venue_Update]
(
	@Success bit output,
	@Id int,
	@VenueName nvarchar(50),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Venue]
	SET
		[VenueName] = @VenueName,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
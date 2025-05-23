-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete Venue
-- =========================================================
CREATE PROCEDURE [dbo].[Venue_Delete]
(
	@Success bit output,
	@VenueID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Venue]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @VenueID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
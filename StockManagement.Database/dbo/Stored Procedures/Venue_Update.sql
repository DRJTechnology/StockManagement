-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update Venue
-- =========================================================
-- 11 Jun 2025 - DB Updated to include Notes field
CREATE PROCEDURE [dbo].[Venue_Update]
(
	@Success bit output,
	@Id int,
	@VenueName nvarchar(50),
	@Notes nvarchar(4000),
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
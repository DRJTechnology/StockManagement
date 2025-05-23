
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Venues
-- =========================================================
CREATE PROCEDURE [dbo].[Venue_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[VenueName],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [Venue]
	WHERE
		[Deleted] <> 1

	SET @Err = @@Error

	RETURN @Err
END
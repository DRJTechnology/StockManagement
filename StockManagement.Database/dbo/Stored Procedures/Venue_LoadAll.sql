
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Venues
-- =========================================================
-- 11 Jun 2025 - DB Updated to include Notes field
CREATE PROCEDURE [dbo].[Venue_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[VenueName],
		[Notes],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [Venue]
	WHERE
		[Deleted] <> 1
	ORDER BY
		[VenueName] ASC

	SET @Err = @@Error

	RETURN @Err
END
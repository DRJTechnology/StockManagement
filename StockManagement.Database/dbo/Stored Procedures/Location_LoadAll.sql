
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Locations
-- =========================================================
CREATE PROCEDURE [dbo].[Location_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[Name],
		[Notes],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [Location]
	WHERE
		[Deleted] <> 1
	ORDER BY
		[Name] ASC

	SET @Err = @@Error

	RETURN @Err
END
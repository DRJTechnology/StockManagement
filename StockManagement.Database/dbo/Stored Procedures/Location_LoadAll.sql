
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Locations
-- =========================================================
-- 10 Sep 2025 - DB Updated to include ContactId
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
		[ContactId],
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
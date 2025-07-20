
-- =========================================================
-- Author:		Dave Brown
-- Create date: 18 Jul 2025
-- Description:	Get Settings
-- =========================================================
CREATE PROCEDURE [dbo].[Setting_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[SettingName],
		[SettingValue],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [Setting]
	WHERE
		[Deleted] <> 1
	ORDER BY
		[SettingName] ASC

	SET @Err = @@Error

	RETURN @Err
END
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Actions
-- =========================================================
CREATE PROCEDURE [dbo].[Action_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[ActionName],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [Action]
	WHERE
		[Deleted] <> 1

	SET @Err = @@Error

	RETURN @Err
END
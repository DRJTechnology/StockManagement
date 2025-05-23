-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Product Types
-- =========================================================
CREATE PROCEDURE [dbo].[ProductType_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[ProductTypeName],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [ProductType]
	WHERE
		[Deleted] <> 1

	SET @Err = @@Error

	RETURN @Err
END
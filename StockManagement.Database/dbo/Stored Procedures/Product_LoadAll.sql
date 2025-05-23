-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Products
-- =========================================================
CREATE PROCEDURE [dbo].[Product_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[ProductName],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [Product]
	WHERE
		[Deleted] <> 1

	SET @Err = @@Error

	RETURN @Err
END
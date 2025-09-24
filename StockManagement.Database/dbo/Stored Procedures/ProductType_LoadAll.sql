-- ============================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Product Types
-- ============================================================
-- 21 Aug 2025 - Dave Brown - Added Default Cost and Sale price
-- ============================================================
CREATE PROCEDURE [dbo].[ProductType_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[ProductTypeName],
		[DefaultCostPrice],
		[DefaultSalePrice],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [ProductType]
	WHERE
		[Deleted] <> 1
	ORDER BY
		[ProductTypeName] ASC

	SET @Err = @@Error

	RETURN @Err
END
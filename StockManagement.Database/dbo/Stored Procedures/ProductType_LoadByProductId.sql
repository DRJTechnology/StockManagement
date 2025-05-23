-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Product Types by product id
-- =========================================================
CREATE PROCEDURE [dbo].[ProductType_LoadByProductId]
(
	@ProductId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		pt.[Id],
		pt.[ProductTypeName],
		pt.[Deleted],
		pt.[AmendUserID],
		pt.[AmendDate]
	FROM [ProductType] pt
	INNER JOIN [ProductProductType] ppt ON pt.Id = ppt.ProductTypeId
	WHERE
		ppt.ProductId = @ProductId
		AND pt.[Deleted] <> 1
		AND ppt.[Deleted] <> 1

	SET @Err = @@Error

	RETURN @Err
END
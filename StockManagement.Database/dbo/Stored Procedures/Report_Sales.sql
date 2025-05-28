-- =========================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Get Sales
-- =========================================================
CREATE PROCEDURE [dbo].[Report_Sales]
	@VenueId int = 0,
	@ProductId int = 0,
	@ProductTypeId int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT v.VenueName, pt.ProductTypeName, p.ProductName, SUM(a.Quantity) AS TotalSales
	FROM dbo.Activity a
	INNER JOIN dbo.Product p ON a.ProductId = p.Id
	INNER JOIN dbo.ProductType pt ON a.ProductTypeId = pt.Id
	INNER JOIN dbo.Venue v ON a.VenueId = v.Id
	WHERE	a.Deleted = 0
		AND	a.ActionId = 5 -- Sale
		AND (@VenueId IS NULL OR a.VenueId = @VenueId)
		AND (@ProductId IS NULL OR a.ProductId = @ProductId)
		AND (@ProductTypeId IS NULL OR a.ProductTypeId = @ProductTypeId)
	GROUP BY v.VenueName, pt.ProductTypeName, p.ProductName
	ORDER BY v.VenueName, pt.ProductTypeName, p.ProductName

	SET @Err = @@Error

	RETURN @Err
END


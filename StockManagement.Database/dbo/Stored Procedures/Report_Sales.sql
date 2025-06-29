-- =========================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Get Sales
-- =========================================================
-- 29 JUN 2025 - Dave Brown - Include 'Totals' option
-- =========================================================
CREATE PROCEDURE [dbo].[Report_Sales]
	@VenueId int = 0,
	@ProductId int = 0,
	@ProductTypeId int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	IF @VenueId = -1 -- Totals
	BEGIN
		SELECT 'Totals' AS VenueName, pt.ProductTypeName, p.ProductName, SUM(a.Quantity) AS TotalSales
		FROM dbo.Activity a
		INNER JOIN dbo.Product p ON a.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON a.ProductTypeId = pt.Id
		INNER JOIN dbo.Venue v ON a.VenueId = v.Id
		WHERE	a.Deleted = 0
			AND	a.ActionId = 5 -- Sale
			AND (@ProductId = 0 OR a.ProductId = @ProductId)
			AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
		GROUP BY pt.ProductTypeName, p.ProductName
		ORDER BY pt.ProductTypeName, p.ProductName
	END
	ELSE
	BEGIN
		SELECT v.VenueName, pt.ProductTypeName, p.ProductName, SUM(a.Quantity) AS TotalSales
		FROM dbo.Activity a
		INNER JOIN dbo.Product p ON a.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON a.ProductTypeId = pt.Id
		INNER JOIN dbo.Venue v ON a.VenueId = v.Id
		WHERE	a.Deleted = 0
			AND	a.ActionId = 5 -- Sale
			AND (@VenueId = 0 OR a.VenueId = @VenueId)
			AND (@ProductId = 0 OR a.ProductId = @ProductId)
			AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
		GROUP BY v.VenueName, pt.ProductTypeName, p.ProductName
		ORDER BY v.VenueName, pt.ProductTypeName, p.ProductName
	END

	SET @Err = @@Error

	RETURN @Err
END


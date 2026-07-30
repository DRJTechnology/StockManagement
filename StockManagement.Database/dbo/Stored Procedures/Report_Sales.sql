-- ==============================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Get Sales
-- ==============================================================
-- 29 JUN 2025 - Dave Brown - Include 'Totals' option
-- 20 JAN 2026 - Dave Brown - Include option to group by Customer
-- 30 JUL 2026 - Dave Brown - By Customer: sales not linked through to a
--                            financial transaction are grouped as 'No Customer'
-- ==============================================================
CREATE PROCEDURE [dbo].[Report_Sales]
	@SalesReportType int = 1, -- 1 = By Location, 2 = By Customer
	@LocationId int = 0,
	@CustomerId int = 0,
	@ProductId int = 0,
	@ProductTypeId int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	IF (@SalesReportType = 1 AND @LocationId = -1) OR (@SalesReportType = 2 AND @CustomerId = -1) -- Totals
	BEGIN
		SELECT 'Totals' AS LocationName, 'Totals' AS CustomerName, pt.ProductTypeName, p.ProductName, SUM(a.Quantity) AS TotalSales
		FROM dbo.Activity a
		INNER JOIN dbo.Product p ON a.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON a.ProductTypeId = pt.Id
		INNER JOIN dbo.Location l ON a.LocationId = l.Id
		WHERE	a.Deleted = 0
			AND	a.ActionId = 5 -- Sale
			AND (@ProductId = 0 OR a.ProductId = @ProductId)
			AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
		GROUP BY pt.ProductTypeName, p.ProductName
		ORDER BY pt.ProductTypeName, p.ProductName
	END
	ELSE IF @SalesReportType = 1 -- By Location
	BEGIN
		SELECT l.[Name] AS LocationName, pt.ProductTypeName, p.ProductName, SUM(a.Quantity) AS TotalSales
		FROM dbo.Activity a
		INNER JOIN dbo.Product p ON a.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON a.ProductTypeId = pt.Id
		INNER JOIN dbo.[Location] l ON a.LocationId = l.Id
		WHERE	a.Deleted = 0
			AND	a.ActionId = 5 -- Sale
			AND (@LocationId = 0 OR a.LocationId = @LocationId)
			AND (@ProductId = 0 OR a.ProductId = @ProductId)
			AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
		GROUP BY l.[Name], pt.ProductTypeName, p.ProductName
		ORDER BY l.[Name], pt.ProductTypeName, p.ProductName
	END
	ELSE IF @SalesReportType = 2 -- By Customer
	BEGIN
		-- Sales that are not linked through to a financial transaction (and therefore
		-- a customer) are grouped together under a 'No Customer' heading.
		SELECT ISNULL(c.[Name], 'No Customer') AS CustomerName, pt.ProductTypeName, p.ProductName, SUM(a.Quantity) AS TotalSales
		FROM dbo.Activity a
		INNER JOIN dbo.Product p ON a.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON a.ProductTypeId = pt.Id
		LEFT JOIN dbo.StockSaleDetail ssd on a.StockSaleDetailId = ssd.Id
		LEFT JOIN finance.TransactionDetail td on ssd.TransactionDetailId = td.Id
		LEFT JOIN dbo.Contact c on td.ContactId = c.Id
		WHERE	a.Deleted = 0
			AND	a.ActionId = 5 -- Sale
			AND (@CustomerId = 0 OR c.Id = @CustomerId)
			AND (@ProductId = 0 OR a.ProductId = @ProductId)
			AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
		GROUP BY c.[Name], pt.ProductTypeName, p.ProductName
		ORDER BY c.[Name], pt.ProductTypeName, p.ProductName
	END

	SET @Err = @@Error

	RETURN @Err
END


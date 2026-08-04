-- ==============================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Get Sales
-- ==============================================================
-- 29 JUN 2025 - Dave Brown - Include 'Totals' option
-- 20 JAN 2026 - Dave Brown - Include option to group by Customer
-- 30 JUL 2026 - Dave Brown - By Customer: sales not linked through to a
--                            financial transaction are grouped as 'No Customer'
-- 02 AUG 2026 - Dave Brown - Added @FromDate/@ToDate so the report can be run
--                            for a financial year, and added SalesValue and
--                            CostValue. The report previously returned
--                            quantities only, for all time, which made it
--                            unusable for preparing accounts.
--                            Existing callers are unaffected: the new
--                            parameters default to the full date range and the
--                            new columns are additive.
-- ==============================================================
CREATE PROCEDURE [dbo].[Report_Sales]
	@SalesReportType int = 1, -- 1 = By Location, 2 = By Customer
	@LocationId int = 0,
	@CustomerId int = 0,
	@ProductId int = 0,
	@ProductTypeId int = 0,
	@FromDate date = '2000-01-01',
	@ToDate date = '2099-12-31'
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	-- Sale activities, with the sale value from the linked sale line and the
	-- stock cost from the batches the sale was fulfilled from. Sales recorded
	-- before the stock/finance module went live are not linked to a sale line
	-- and so carry no value.
	;WITH SaleLines AS (
		SELECT
			a.Id AS ActivityId,
			a.ProductId,
			a.ProductTypeId,
			a.LocationId,
			a.Quantity,
			c.Id AS CustomerId,
			c.[Name] AS CustomerName,
			ISNULL(ssd.UnitPrice, 0) * a.Quantity AS SalesValue,
			ISNULL((SELECT SUM(iba.Quantity * ib.UnitCost)
					FROM finance.InventoryBatchActivity iba
					INNER JOIN finance.InventoryBatch ib ON iba.InventoryBatchId = ib.Id
					WHERE iba.ActivityId = a.Id AND iba.Deleted = 0 AND ib.Deleted = 0), 0) AS CostValue
		FROM dbo.Activity a
		LEFT JOIN dbo.StockSaleDetail ssd        ON a.StockSaleDetailId = ssd.Id AND ssd.Deleted = 0
		LEFT JOIN finance.TransactionDetail td   ON ssd.TransactionDetailId = td.Id AND td.Deleted = 0
		LEFT JOIN dbo.Contact c                  ON td.ContactId = c.Id
		WHERE a.Deleted = 0
		  AND a.ActionId = 5 -- Sale
		  AND a.ActivityDate >= @FromDate
		  AND a.ActivityDate <= @ToDate
		  AND (@ProductId = 0 OR a.ProductId = @ProductId)
		  AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
	)
	SELECT LocationName, CustomerName, ProductTypeName, ProductName, TotalSales, SalesValue, CostValue
	FROM (
		SELECT 'Totals' AS LocationName, 'Totals' AS CustomerName,
			   pt.ProductTypeName, p.ProductName,
			   SUM(sl.Quantity) AS TotalSales,
			   SUM(sl.SalesValue) AS SalesValue,
			   SUM(sl.CostValue) AS CostValue,
			   1 AS Branch
		FROM SaleLines sl
		INNER JOIN dbo.Product p      ON sl.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON sl.ProductTypeId = pt.Id
		WHERE (@SalesReportType = 1 AND @LocationId = -1)
		   OR (@SalesReportType = 2 AND @CustomerId = -1)
		GROUP BY pt.ProductTypeName, p.ProductName

		UNION ALL

		SELECT l.[Name], NULL, pt.ProductTypeName, p.ProductName,
			   SUM(sl.Quantity), SUM(sl.SalesValue), SUM(sl.CostValue), 2
		FROM SaleLines sl
		INNER JOIN dbo.Product p      ON sl.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON sl.ProductTypeId = pt.Id
		INNER JOIN dbo.[Location] l   ON sl.LocationId = l.Id
		WHERE @SalesReportType = 1 AND @LocationId <> -1
		  AND (@LocationId = 0 OR sl.LocationId = @LocationId)
		GROUP BY l.[Name], pt.ProductTypeName, p.ProductName

		UNION ALL

		-- Sales not linked through to a financial transaction (and therefore a
		-- customer) are grouped together under a 'No Customer' heading.
		SELECT NULL, ISNULL(sl.CustomerName, 'No Customer'), pt.ProductTypeName, p.ProductName,
			   SUM(sl.Quantity), SUM(sl.SalesValue), SUM(sl.CostValue), 3
		FROM SaleLines sl
		INNER JOIN dbo.Product p      ON sl.ProductId = p.Id
		INNER JOIN dbo.ProductType pt ON sl.ProductTypeId = pt.Id
		WHERE @SalesReportType = 2 AND @CustomerId <> -1
		  AND (@CustomerId = 0 OR sl.CustomerId = @CustomerId)
		GROUP BY sl.CustomerName, pt.ProductTypeName, p.ProductName
	) x
	ORDER BY LocationName, CustomerName, ProductTypeName, ProductName

	SET @Err = @@Error

	RETURN @Err
END

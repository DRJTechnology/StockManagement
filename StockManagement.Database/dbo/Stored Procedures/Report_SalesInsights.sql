
-- ==============================================================
-- Author:		Dave Brown
-- Create date: 21 Aug 2026
-- Description:	Sale lines, unaggregated, for the Sales Insights page.
-- ==============================================================
-- Report_Sales collapses the whole period into one row per product, so it can
-- give neither a trend over time nor a count of sales. This returns the same
-- sale lines flat - one row per sale activity - and lets the page aggregate
-- them into KPIs, charts and rankings without a round trip per view.
--
-- The SaleLines CTE is deliberately identical to the one in Report_Sales so
-- the two reports reconcile. If one changes, change both.
-- ==============================================================
CREATE PROCEDURE [dbo].[Report_SalesInsights]
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
			a.ActivityDate,
			ssd.StockSaleId,
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
	SELECT
		sl.ActivityDate AS SaleDate,
		sl.StockSaleId,
		sl.LocationId,
		l.[Name] AS LocationName,
		sl.CustomerId,
		-- Sales not linked through to a financial transaction have no customer.
		ISNULL(sl.CustomerName, 'No Customer') AS CustomerName,
		sl.ProductTypeId,
		pt.ProductTypeName,
		sl.ProductId,
		p.ProductName,
		sl.Quantity,
		sl.SalesValue,
		sl.CostValue
	FROM SaleLines sl
	INNER JOIN dbo.Product p      ON sl.ProductId = p.Id
	INNER JOIN dbo.ProductType pt ON sl.ProductTypeId = pt.Id
	INNER JOIN dbo.[Location] l   ON sl.LocationId = l.Id
	WHERE (@LocationId = 0 OR sl.LocationId = @LocationId)
	  AND (@CustomerId = 0 OR sl.CustomerId = @CustomerId)
	ORDER BY sl.ActivityDate, sl.ActivityId

	SET @Err = @@Error

	RETURN @Err
END

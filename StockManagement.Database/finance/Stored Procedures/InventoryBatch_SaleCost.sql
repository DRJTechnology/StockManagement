-- =======================================================================
-- Author:		Dave Brown
-- Create date: 23 Sep 2025
-- Description:	Stock cost of a sale
-- =======================================================================
CREATE PROCEDURE [finance].[InventoryBatch_SaleCost]
	@StockSaleId INT,
	@SaleCost MONEY OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	@SaleCost = SUM(iba.Quantity * ib.UnitCost)
	FROM	finance.InventoryBatchActivity iba
	INNER JOIN finance.InventoryBatch ib on iba.InventoryBatchId = ib.Id
	INNER JOIN dbo.Activity a ON iba.ActivityId = a.Id
	INNER JOIN StockSaleDetail ssd ON a.StockSaleDetailId = ssd.Id
	WHERE	ssd.StockSaleId = @StockSaleId
		AND iba.Deleted = 0
		AND ib.Deleted = 0
		AND a.Deleted = 0
		AND ssd.Deleted = 0

END

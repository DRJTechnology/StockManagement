-- =======================================================================
-- Author:		Dave Brown
-- Create date: 20 Sep 2025
-- Description:	Current Inventory Value
-- =======================================================================
CREATE PROCEDURE [finance].[Report_InventoryValue]
	@TotalValue money OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- Calculate total value of active, non-deleted inventory batches
	SELECT
		@TotalValue = SUM(Quantityremaining * UnitCost)
	FROM
		finance.InventoryBatch
	WHERE
		Deleted = 0
		AND InventoryBatchStatusId = 2 -- 2 = Active
END

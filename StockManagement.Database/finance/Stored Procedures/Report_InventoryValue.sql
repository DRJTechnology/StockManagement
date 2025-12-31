-- =======================================================================
-- Author:		Dave Brown
-- Create date: 20 Sep 2025
-- Description:	Current Inventory Value
-- =======================================================================
-- 03 Oct 2025 - Dave Brown - Include Pending inventory in total value
-- =======================================================================
CREATE PROCEDURE [finance].[Report_InventoryValue]
	@TotalActiveValue money OUTPUT,
	@TotalValue money OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- Calculate total value of active, non-deleted inventory batches
	SELECT
		@TotalActiveValue = SUM(Quantityremaining * UnitCost)
	FROM
		finance.InventoryBatch
	WHERE
		Deleted = 0
		AND InventoryBatchStatusId = 2 -- 2 = Active

	-- Calculate total value of active/pending, non-deleted inventory batches
	SELECT
		@TotalValue = SUM(Quantityremaining * UnitCost)
	FROM
		finance.InventoryBatch
	WHERE
		Deleted = 0
		AND InventoryBatchStatusId IN (1, 2) -- 1 = Pending, 2 = Active
END

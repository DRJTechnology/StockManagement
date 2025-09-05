-- =======================================================================
-- Author:		Dave Brown
-- Create date: 04 Sep 2025
-- Description:	Get Inventory Batch Activity records by inventory batch id
-- =======================================================================
CREATE PROCEDURE [finance].[InventoryBatchActivity_LoadByInventoryBatchId]
    @InventoryBatchId int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Err int;

    SELECT  iba.Id, iba.Quantity, iba.ActivityId, a.ActivityDate, atn.ActionName, l.[Name] as LocationName, a.Notes, sod.StockOrderId, ssd.StockSaleId
    FROM    finance.inventorybatchactivity iba
	INNER JOIN dbo.Activity a on iba.ActivityId = a.Id
	INNER JOIN dbo.[action] atn on a.ActionId = atn.Id
	INNER JOIN dbo.[Location] l on a.LocationId = l.Id
    LEFT OUTER JOIN dbo.[StockOrderDetail] sod on a.StockOrderDetailId = sod.Id
    LEFT OUTER JOIN dbo.[StockSaleDetail] ssd on a.StockSaleDetailId = ssd.Id
    WHERE   iba.Deleted = 0
        AND inventoryBatchid = @InventoryBatchId
    ORDER BY a.ActivityDate DESC, iba.Id DESC

    SET @Err = @@Error;
    RETURN @Err;
END
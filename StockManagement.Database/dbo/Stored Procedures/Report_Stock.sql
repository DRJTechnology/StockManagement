-- =========================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Get Stock levels
-- =========================================================
-- 25 JUN 2025 - Dave Brown - Do not display zero TotalQuantity
-- 29 JUN 2025 - Dave Brown - Include 'Totals' option
-- 06 JUL 2025 - Dave Brown - Show 0 stock for stockroom and cards
-- 29 Aug 2025 - Dave Brown - Updated to get totals from the Inventory batch records
-- =========================================================
CREATE PROCEDURE [dbo].[Report_Stock]
	@LocationId int = 0,
	@ProductId int = 0,
	@ProductTypeId int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int;

    IF @LocationId = -1 -- Totals (not grouped by Location)
    BEGIN
        SELECT  -1 AS LocationId, 'Totals' AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 2 THEN ib.QuantityRemaining ELSE 0 END) AS ActiveQuantity,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 1 THEN ib.QuantityRemaining ELSE 0 END) AS PendingQuantity
        FROM    finance.InventoryBatch ib
        INNER JOIN Product p ON ib.ProductId = p.Id
        INNER JOIN ProductType pt ON ib.ProductTypeId = pt.Id
        INNER JOIN Location l ON ib.LocationId = l.Id
        WHERE ib.Deleted = 0 AND ib.QuantityRemaining > 0
            AND (@ProductId = 0 OR ib.ProductId = @ProductId)
            AND (@ProductTypeId = 0 OR ib.ProductTypeId = @ProductTypeId)
        GROUP BY pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ib.QuantityRemaining) > 0
        ORDER BY pt.ProductTypeName, p.ProductName
    END
    ELSE IF @LocationId = 0 -- No location selected
    BEGIN
        SELECT  l.Id AS LocationId, l.[Name] AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 2 THEN ib.QuantityRemaining ELSE 0 END) AS ActiveQuantity,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 1 THEN ib.QuantityRemaining ELSE 0 END) AS PendingQuantity
        FROM    finance.InventoryBatch ib
        INNER JOIN Product p ON ib.ProductId = p.Id
        INNER JOIN ProductType pt ON ib.ProductTypeId = pt.Id
        INNER JOIN Location l ON ib.LocationId = l.Id
        WHERE ib.Deleted = 0 AND ib.QuantityRemaining > 0
            AND (@ProductId = 0 OR ib.ProductId = @ProductId)
            AND (@ProductTypeId = 0 OR ib.ProductTypeId = @ProductTypeId)
        GROUP BY l.Id, l.[Name], pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ib.QuantityRemaining) > 0
        ORDER BY l.[Name], pt.ProductTypeName, p.ProductName
    END
    ELSE IF @LocationId = 1 -- Stockroom
    BEGIN
        SELECT  l.Id AS LocationId, l.[Name] AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 2 THEN ib.QuantityRemaining ELSE 0 END) AS ActiveQuantity,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 1 THEN ib.QuantityRemaining ELSE 0 END) AS PendingQuantity
        FROM    finance.InventoryBatch ib
        INNER JOIN Product p ON ib.ProductId = p.Id
        INNER JOIN ProductType pt ON ib.ProductTypeId = pt.Id
        INNER JOIN Location l ON ib.LocationId = l.Id
        WHERE ib.Deleted = 0 AND ib.QuantityRemaining > 0
            AND (@LocationId = 0 OR ib.LocationId = @LocationId)
            AND (@ProductId = 0 OR ib.ProductId = @ProductId)
            AND (@ProductTypeId = 0 OR ib.ProductTypeId = @ProductTypeId)
        GROUP BY l.Id, l.[Name], pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ib.QuantityRemaining) > 0 OR (l.Id = 1 AND pt.Id = 2) -- Show Zero if Stockroom and Card
        ORDER BY l.[Name], pt.ProductTypeName, p.ProductName
    END
    ELSE
    BEGIN -- Non-stockroom location selected
        SELECT  l.Id AS LocationId, l.[Name] AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 2 THEN ib.QuantityRemaining ELSE 0 END) AS ActiveQuantity,
                SUM(CASE WHEN ib.InventoryBatchStatusId = 1 THEN ib.QuantityRemaining ELSE 0 END) AS PendingQuantity
        FROM    finance.InventoryBatch ib
        INNER JOIN Product p ON ib.ProductId = p.Id
        INNER JOIN ProductType pt ON ib.ProductTypeId = pt.Id
        INNER JOIN Location l ON ib.LocationId = l.Id
        WHERE ib.Deleted = 0 AND ib.QuantityRemaining > 0
            AND (@LocationId = 0 OR ib.LocationId = @LocationId)
            AND (@ProductId = 0 OR ib.ProductId = @ProductId)
            AND (@ProductTypeId = 0 OR ib.ProductTypeId = @ProductTypeId)
        GROUP BY l.Id, l.[Name], pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ib.QuantityRemaining) > 0 OR (l.Id = 1 AND pt.Id = 2) -- Show Zero if Stockroom and Card
        ORDER BY l.[Name], pt.ProductTypeName, p.ProductName
    END

	SET @Err = @@Error

	RETURN @Err
END
-- =============================================================================
-- Author:      Dave Brown
-- Create date: 02 Aug 2026
-- Description: Closing stock valuation as at a given date.
--
--              Report_InventoryValue only ever valued stock as at *now*, so
--              there was no way to produce the closing stock figure the
--              accountant needs at a year end. This rebuilds each batch's
--              quantity from its movement history (see
--              finance.vw_InventoryBatchMovement) and values what was on hand
--              at @AsAtDate.
--
--              Stock is valued at cost (FIFO unit cost of the batch). Market
--              value is also returned, from ProductType.DefaultSalePrice, so
--              the accountant can consider lower of cost and net realisable
--              value, and so stock taken for personal use can be reviewed.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_StockValuation]
    @AsAtDate       DATE,
    @LocationId     INT = 0,    -- 0 = all
    @ProductTypeId  INT = 0,    -- 0 = all
    @ProductId      INT = 0     -- 0 = all
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH BatchQty AS (
        SELECT m.InventoryBatchId, m.ProductId, m.ProductTypeId, m.LocationId, m.UnitCost,
               SUM(m.SignedQty) AS Quantity
        FROM finance.vw_InventoryBatchMovement m
        WHERE m.ActivityDate <= @AsAtDate
        GROUP BY m.InventoryBatchId, m.ProductId, m.ProductTypeId, m.LocationId, m.UnitCost
        HAVING SUM(m.SignedQty) > 0
    )
    SELECT
        l.Id                AS LocationId,
        l.[Name]            AS LocationName,
        pt.Id               AS ProductTypeId,
        pt.ProductTypeName,
        p.Id                AS ProductId,
        p.ProductName,
        SUM(bq.Quantity)                                       AS Quantity,
        SUM(bq.Quantity * bq.UnitCost)                         AS CostValue,
        SUM(bq.Quantity * ISNULL(pt.DefaultSalePrice, 0))      AS MarketValue
    FROM BatchQty bq
    INNER JOIN dbo.Product     p  ON bq.ProductId     = p.Id
    INNER JOIN dbo.ProductType pt ON bq.ProductTypeId = pt.Id
    INNER JOIN dbo.[Location]  l  ON bq.LocationId    = l.Id
    WHERE (@LocationId    = 0 OR bq.LocationId    = @LocationId)
      AND (@ProductTypeId = 0 OR bq.ProductTypeId = @ProductTypeId)
      AND (@ProductId     = 0 OR bq.ProductId     = @ProductId)
    GROUP BY l.Id, l.[Name], pt.Id, pt.ProductTypeName, p.Id, p.ProductName
    ORDER BY l.[Name], pt.ProductTypeName, p.ProductName;
END

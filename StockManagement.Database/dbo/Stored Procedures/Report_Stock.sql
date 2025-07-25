-- =========================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Get Stock levels
-- =========================================================
-- 25 JUN 2025 - Dave Brown - Do not display zero TotalQuantity
-- 29 JUN 2025 - Dave Brown - Include 'Totals' option
-- 06 JUL 2025 - Dave Brown - Show 0 stock for stockroom and cards
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
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.LocationId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
            UNION ALL
            SELECT a.ActivityDate, a.ProductId, a.ProductTypeId, 1 /* Stockroom */ as LocationId, a.Quantity * act.Direction * -1 AS Quantity
            FROM dbo.[Activity] a
            INNER JOIN dbo.[Action] act ON a.ActionId = act.Id
            WHERE act.AffectStockRoom = 1
                AND a.Deleted = 0
                AND (@ProductId = 0 OR a.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT -1 AS LocationId, 'Totals' AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        GROUP BY pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0
        ORDER BY pt.ProductTypeName, p.ProductName
    END
    ELSE IF @LocationId = 0 -- No location selected
    BEGIN
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.LocationId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@LocationId = 0 OR a1.LocationId = @LocationId)
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
            UNION ALL
            SELECT a.ActivityDate, a.ProductId, a.ProductTypeId, 1 /* Stockroom */ as LocationId, a.Quantity * act.Direction * -1 AS Quantity
            FROM dbo.[Activity] a
            INNER JOIN dbo.[Action] act ON a.ActionId = act.Id
            WHERE act.AffectStockRoom = 1
                AND a.Deleted = 0
                AND (@LocationId = 0 OR a.LocationId = @LocationId)
                AND (@ProductId = 0 OR a.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT l.Id AS LocationId, l.[Name] AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        INNER JOIN dbo.[Location] l ON ca.LocationId = l.Id
        GROUP BY l.Id, l.[Name], pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0
        ORDER BY l.[Name], pt.ProductTypeName, p.ProductName
    END
    ELSE IF @LocationId = 1 -- Stockroom
    BEGIN
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.LocationId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@LocationId = 0 OR a1.LocationId = @LocationId)
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
            UNION ALL
            SELECT a.ActivityDate, a.ProductId, a.ProductTypeId, 1 /* Stockroom */ as LocationId, a.Quantity * act.Direction * -1 AS Quantity
            FROM dbo.[Activity] a
            INNER JOIN dbo.[Action] act ON a.ActionId = act.Id
            WHERE act.AffectStockRoom = 1
                AND a.Deleted = 0
                AND (@ProductId = 0 OR a.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT l.Id AS LocationId, l.[Name] AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        INNER JOIN dbo.[Location] l ON ca.LocationId = l.Id
        GROUP BY l.Id, l.[Name], pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0 OR (l.Id = 1 AND pt.Id = 2) -- Show Zero if Stockroom and Card
        ORDER BY l.[Name], pt.ProductTypeName, p.ProductName
    END
    ELSE
    BEGIN -- Non-stockroom location selected
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.LocationId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@LocationId = 0 OR a1.LocationId = @LocationId)
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT l.Id AS LocationId, l.[Name] AS LocationName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        INNER JOIN dbo.[Location] l ON ca.LocationId = l.Id
        GROUP BY l.Id, l.[Name], pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0
        ORDER BY l.[Name], pt.ProductTypeName, p.ProductName
    END

	SET @Err = @@Error

	RETURN @Err
END
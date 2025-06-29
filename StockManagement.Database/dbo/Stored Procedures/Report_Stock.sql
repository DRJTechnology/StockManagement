-- =========================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Get Stock levels
-- =========================================================
-- 25 JUN 2025 - Dave Brown - Do not display zero TotalQuantity
-- 29 JUN 2025 - Dave Brown - Include 'Totals' option
-- =========================================================
CREATE PROCEDURE [dbo].[Report_Stock]
	@VenueId int = 0,
	@ProductId int = 0,
	@ProductTypeId int = 0
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int;

    IF @VenueId = -1 -- Totals (not grouped by Venue)
    BEGIN
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.VenueId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
            UNION ALL
            SELECT a.ActivityDate, a.ProductId, a.ProductTypeId, 1 /* Stockroom */ as VenueId, a.Quantity * act.Direction * -1 AS Quantity
            FROM dbo.[Activity] a
            INNER JOIN dbo.[Action] act ON a.ActionId = act.Id
            WHERE act.AffectStockRoom = 1
                AND a.Deleted = 0
                AND (@ProductId = 0 OR a.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT -1 AS VenueId, 'Totals' AS VenueName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        GROUP BY pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0
        ORDER BY pt.ProductTypeName, p.ProductName
    END
    ELSE IF @VenueId = 0 -- No venue selected
    BEGIN
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.VenueId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@VenueId = 0 OR a1.VenueId = @VenueId)
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
            UNION ALL
            SELECT a.ActivityDate, a.ProductId, a.ProductTypeId, 1 /* Stockroom */ as VenueId, a.Quantity * act.Direction * -1 AS Quantity
            FROM dbo.[Activity] a
            INNER JOIN dbo.[Action] act ON a.ActionId = act.Id
            WHERE act.AffectStockRoom = 1
                AND a.Deleted = 0
                AND (@VenueId = 0 OR a.VenueId = @VenueId)
                AND (@ProductId = 0 OR a.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT v.Id AS VenueId, v.VenueName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        INNER JOIN dbo.Venue v ON ca.VenueId = v.Id
        GROUP BY v.Id, v.VenueName, pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0
        ORDER BY v.VenueName, pt.ProductTypeName, p.ProductName
    END
    ELSE IF @VenueId = 1 -- Stockroom
    BEGIN
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.VenueId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@VenueId = 0 OR a1.VenueId = @VenueId)
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
            UNION ALL
            SELECT a.ActivityDate, a.ProductId, a.ProductTypeId, 1 /* Stockroom */ as VenueId, a.Quantity * act.Direction * -1 AS Quantity
            FROM dbo.[Activity] a
            INNER JOIN dbo.[Action] act ON a.ActionId = act.Id
            WHERE act.AffectStockRoom = 1
                AND a.Deleted = 0
                AND (@ProductId = 0 OR a.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT v.Id AS VenueId, v.VenueName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        INNER JOIN dbo.Venue v ON ca.VenueId = v.Id
        GROUP BY v.Id, v.VenueName, pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0
        ORDER BY v.VenueName, pt.ProductTypeName, p.ProductName
    END
    ELSE
    BEGIN -- Non-stockroom venue selected
        WITH CombinedActivity AS (
            SELECT a1.ActivityDate, a1.ProductId, a1.ProductTypeId, a1.VenueId, a1.Quantity * a2.Direction AS Quantity
            FROM dbo.Activity a1
	        INNER JOIN dbo.[Action] a2 ON a1.ActionId = a2.Id 
            WHERE a1.Deleted = 0
                AND (@VenueId = 0 OR a1.VenueId = @VenueId)
                AND (@ProductId = 0 OR a1.ProductId = @ProductId)
                AND (@ProductTypeId = 0 OR a1.ProductTypeId = @ProductTypeId)
        )

        -- Perform the GROUP BY on the combined results
        SELECT v.Id AS VenueId, v.VenueName, pt.Id AS ProductTypeId, pt.ProductTypeName, p.Id AS ProductId, p.ProductName, SUM(ca.Quantity) AS TotalQuantity
        FROM CombinedActivity ca
        INNER JOIN dbo.Product p ON ca.ProductId = p.Id
        INNER JOIN dbo.ProductType pt ON ca.ProductTypeId = pt.Id
        INNER JOIN dbo.Venue v ON ca.VenueId = v.Id
        GROUP BY v.Id, v.VenueName, pt.Id, pt.ProductTypeName, p.Id, p.ProductName
        HAVING SUM(ca.Quantity) > 0
        ORDER BY v.VenueName, pt.ProductTypeName, p.ProductName
    END

	SET @Err = @@Error

	RETURN @Err
END


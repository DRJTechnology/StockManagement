-- =========================================================
-- Author:		Dave Brown
-- Create date: May 2025
-- Description:	Get Activity
-- =========================================================
-- 04 Jul 2025 - Dave Brown - DeliveryNoteId added
-- 09 Jul 2025 - Dave Brown - StockNoteId added
-- 30 Aug 2025 - Dave Brown - Added Notes
-- =========================================================
CREATE PROCEDURE [dbo].[Activity_LoadFiltered]
    @ActivityDate datetime = NULL,
    @ActionId int = NULL,
    @ProductId int = NULL,
    @ProductTypeId int = NULL,
    @LocationId int = NULL,
    @Quantity int = NULL,
    @CurrentPage int = 1,
    @PageSize int = 20,
    @TotalPages int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Err int;
    DECLARE @TotalCount int;

    -- Calculate total count
    SELECT @TotalCount = COUNT(*)
    FROM [Activity] a
    WHERE
        a.[Deleted] <> 1
        AND (@ActivityDate IS NULL OR CAST(a.[ActivityDate] AS DATE) = CAST(@ActivityDate AS DATE))
        AND (@ActionId IS NULL OR a.[ActionId] = @ActionId)
        AND (@ProductId IS NULL OR a.[ProductId] = @ProductId)
        AND (@ProductTypeId IS NULL OR a.[ProductTypeId] = @ProductTypeId)
        AND (@LocationId IS NULL OR a.[LocationId] = @LocationId)
        AND (@Quantity IS NULL OR a.[Quantity] = @Quantity);

    -- Calculate total pages
    SELECT @TotalPages = 
        CASE 
            WHEN @TotalCount = 0 THEN 1
            ELSE CEILING(CAST(@TotalCount AS float) / @PageSize)
        END;

    -- Return paged result set directly
    SELECT
        a.[Id],
        a.[ActivityDate],
        a.[ActionId],
        act.[ActionName],
        a.[ProductId],
        p.[ProductName],
        a.[ProductTypeId],
        pt.[ProductTypeName],
        a.[LocationId],
        l.[Name] AS LocationName,
        a.[Quantity],
		a.[Notes],
        dnd.DeliveryNoteId,
        srd.StockOrderId,
        a.[Deleted],
        a.[AmendUserID],
        a.[AmendDate]
    FROM [Activity] a
    INNER JOIN [Product] p ON a.[ProductId] = p.Id
    INNER JOIN [ProductType] pt ON a.[ProductTypeId] = pt.Id
    INNER JOIN [Location] l ON a.[LocationId] = l.Id
    INNER JOIN [Action] act ON a.[ActionId] = act.Id
    LEFT OUTER JOIN [DeliveryNoteDetail] dnd ON a.DeliveryNoteDetailId = dnd.Id
    LEFT OUTER JOIN [StockOrderDetail] srd ON a.StockOrderDetailId = srd.Id
    WHERE
        a.[Deleted] <> 1
        AND (@ActivityDate IS NULL OR CAST(a.[ActivityDate] AS DATE) = CAST(@ActivityDate AS DATE))
        AND (@ActionId IS NULL OR a.[ActionId] = @ActionId)
        AND (@ProductId IS NULL OR a.[ProductId] = @ProductId)
        AND (@ProductTypeId IS NULL OR a.[ProductTypeId] = @ProductTypeId)
        AND (@LocationId IS NULL OR a.[LocationId] = @LocationId)
        AND (@Quantity IS NULL OR a.[Quantity] = @Quantity)
    ORDER BY
        a.[ActivityDate] DESC,
        a.Id DESC
        --p.[ProductName] ASC,
        --pt.[ProductTypeName] ASC,
        --l.[Name] ASC
    OFFSET (@CurrentPage - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SET @Err = @@Error;
    RETURN @Err;
END
-- =====================================================================
-- Author:		Dave Brown
-- Create date: 03 Sep 2025
-- Description:	Get Inventory Batch records with filtering and paging
-- =====================================================================
CREATE PROCEDURE [finance].[InventoryBatch_LoadFiltered]
    @InventoryBatchStatusId int,
    @ProductId int = NULL,
    @ProductTypeId int = NULL,
    @LocationId int = NULL,
    @PurchaseDate datetime = NULL,
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
    FROM [finance].[InventoryBatch] ib
    WHERE
        ib.[Deleted] <> 1
        AND (@InventoryBatchStatusId =0 OR ib.InventoryBatchStatusId = @InventoryBatchStatusId)
        AND (@ProductId IS NULL OR ib.[ProductId] = @ProductId)
        AND (@ProductTypeId IS NULL OR ib.[ProductTypeId] = @ProductTypeId)
        AND (@LocationId IS NULL OR ib.[LocationId] = @LocationId)
        AND (@PurchaseDate IS NULL OR CAST(ib.[PurchaseDate] AS DATE) = CAST(@PurchaseDate AS DATE));

    -- Calculate total pages
    SELECT @TotalPages = 
        CASE 
            WHEN @TotalCount = 0 THEN 1
            ELSE CEILING(CAST(@TotalCount AS float) / @PageSize)
        END;

    -- Return paged result set directly
    SELECT
        ib.[Id],
        ib.[InventoryBatchStatusId],
        ib.[ProductId],
        p.[ProductName],
        ib.[ProductTypeId],
        pt.[ProductTypeName],
        ib.[LocationId],
        l.[Name] AS LocationName,
        ib.[InitialQuantity],
		ib.[QuantityRemaining],
        ib.[UnitCost],
        ib.[PurchaseDate],
        ib.[Deleted],
        ib.[AmendUserID],
        ib.[AmendDate]
    FROM [finance].[InventoryBatch] ib
    INNER JOIN [Product] p ON ib.[ProductId] = p.Id
    INNER JOIN [ProductType] pt ON ib.[ProductTypeId] = pt.Id
    INNER JOIN [Location] l ON ib.[LocationId] = l.Id
    WHERE
        ib.[Deleted] <> 1
        AND (@InventoryBatchStatusId =0 OR ib.InventoryBatchStatusId = @InventoryBatchStatusId)
        AND (@ProductId IS NULL OR ib.[ProductId] = @ProductId)
        AND (@ProductTypeId IS NULL OR ib.[ProductTypeId] = @ProductTypeId)
        AND (@LocationId IS NULL OR ib.[LocationId] = @LocationId)
        AND (@PurchaseDate IS NULL OR CAST(ib.[PurchaseDate] AS DATE) = CAST(@PurchaseDate AS DATE))
    ORDER BY
        ib.[AmendDate] DESC
    OFFSET (@CurrentPage - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SET @Err = @@Error;
    RETURN @Err;
END
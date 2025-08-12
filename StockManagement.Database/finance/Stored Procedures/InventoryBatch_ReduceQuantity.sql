-- =========================================================================
-- Author:		Dave Brown
-- Create date: 10 Aug 2025
-- Description:	Reduces Inventory Batch Quantity using FIFO method
-- =========================================================================
CREATE PROCEDURE [finance].[InventoryBatch_ReduceQuantity]
    @ProductId INT,
    @ProductTypeId INT,
    @LocationId INT,
    @Quantity INT,
    @ActivityId INT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @QtyToDeduct INT = @Quantity;
    DECLARE @BatchId INT;
    DECLARE @BatchQty INT;
    DECLARE @DeductNow INT;

    DECLARE curBatches CURSOR LOCAL FAST_FORWARD FOR
        SELECT Id, QuantityRemaining
        FROM finance.InventoryBatch
        WHERE ProductId = @ProductId
          AND ProductTypeId = @ProductTypeId
          AND LocationId = @LocationId
          AND QuantityRemaining > 0
          AND Deleted = 0
        ORDER BY PurchaseDate ASC, Id ASC;

    OPEN curBatches;
    FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty;

    WHILE @@FETCH_STATUS = 0 AND @QtyToDeduct > 0
    BEGIN
        SET @DeductNow = CASE WHEN @BatchQty >= @QtyToDeduct THEN @QtyToDeduct ELSE @BatchQty END;

        -- Deduct stock
        UPDATE finance.InventoryBatch
        SET QuantityRemaining = QuantityRemaining - @DeductNow,
            AmendUserId = @UserId,
            AmendDate = GETDATE()
        WHERE Id = @BatchId;

        -- Record activity
        INSERT INTO finance.InventoryBatchActivity (InventoryBatchId, ActivityId, CreateUserId, AmendUserId)
        VALUES (@BatchId, @ActivityId, @UserId, @UserId);

        SET @QtyToDeduct = @QtyToDeduct - @DeductNow;

        FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty;
    END

    CLOSE curBatches;
    DEALLOCATE curBatches;

    IF @QtyToDeduct > 0
    BEGIN
        RAISERROR('Not enough stock to fulfil request. ActivityId=%d', 16, 1, @ActivityId);
    END
END
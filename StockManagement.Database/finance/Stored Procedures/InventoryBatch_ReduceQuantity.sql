-- =======================================================================================
-- Author:		Dave Brown
-- Create date: 10 Aug 2025
-- Description:	Reduces Inventory Batch Quantity using FIFO method
-- =======================================================================================
-- 18 Aug 2025 - Added @CostRemoved output parameter to return total cost of removed stock
-- =======================================================================================
CREATE PROCEDURE [finance].[InventoryBatch_ReduceQuantity]
    @ProductId INT,
    @ProductTypeId INT,
    @LocationId INT,
    @Quantity INT,
    @ActivityId INT,
    @UserId INT,
    @CostRemoved MONEY OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @QtyToDeduct INT = @Quantity;
    DECLARE @BatchId INT;
    DECLARE @BatchQty INT;
    DECLARE @UnitCost MONEY;
    DECLARE @DeductNow INT;

    SET @CostRemoved = 0;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE curBatches CURSOR LOCAL FAST_FORWARD FOR
            SELECT Id, QuantityRemaining, UnitCost
            FROM finance.InventoryBatch
            WHERE ProductId = @ProductId
              AND ProductTypeId = @ProductTypeId
              AND LocationId = @LocationId
              AND QuantityRemaining > 0
              AND InventoryBatchStatusId = 2 -- Active
              AND Deleted = 0
            ORDER BY PurchaseDate ASC, Id ASC;

        OPEN curBatches;
        FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty, @UnitCost;

        WHILE @@FETCH_STATUS = 0 AND @QtyToDeduct > 0
        BEGIN
            SET @DeductNow = CASE WHEN @BatchQty >= @QtyToDeduct THEN @QtyToDeduct ELSE @BatchQty END;
            SET @CostRemoved = @CostRemoved + (@DeductNow * @UnitCost);

            -- Deduct stock
            UPDATE finance.InventoryBatch
            SET InventoryBatchStatusId = CASE WHEN @QtyToDeduct < @BatchQty THEN 2 /* Active */ ELSE 3 /* Depleted */ END,
                QuantityRemaining = QuantityRemaining - @DeductNow,
                AmendUserId = @UserId,
                AmendDate = GETDATE()
            WHERE Id = @BatchId;

            -- Record activity
            INSERT INTO finance.InventoryBatchActivity (InventoryBatchId, ActivityId, Quantity, CreateUserId, AmendUserId)
            VALUES (@BatchId, @ActivityId, @DeductNow, @UserId, @UserId);

            SET @QtyToDeduct = @QtyToDeduct - @DeductNow;

            FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty, @UnitCost;
        END

        CLOSE curBatches;
        DEALLOCATE curBatches;

        IF @QtyToDeduct > 0
        BEGIN
            RAISERROR('Not enough stock to fulfil request. ActivityId=%d', 16, 1, @ActivityId);
        END

        -- If we get here, all operations succeeded
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Close and deallocate cursor if still open
        IF CURSOR_STATUS('local', 'curBatches') >= 0
        BEGIN
            CLOSE curBatches;
            DEALLOCATE curBatches;
        END

        -- Rollback the transaction
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Re-raise the error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
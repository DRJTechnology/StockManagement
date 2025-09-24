-- =======================================================================================
-- Author:		Dave Brown
-- Create date: 10 Aug 2025
-- Description:	Moves Inventory Batch Quantity between locations using FIFO method
-- =======================================================================================
CREATE PROCEDURE [finance].[InventoryBatch_MoveQuantity]
    @ProductId INT,
    @ProductTypeId INT,
    @FromLocationId INT,
    @ToLocationId INT,
    @Quantity INT,
    @ActivityId INT,
    @CurrentUserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @QtyToMove INT = @Quantity;
    DECLARE @BatchId INT;
    DECLARE @BatchQty INT;
    DECLARE @UnitCost MONEY;
    DECLARE @PurchaseDate DATETIME;
    DECLARE @OriginalInventoryBatchId INT;
    DECLARE @DeductNow INT;
    DECLARE @UpdateDate DATETIME = GETDATE();

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE curBatches CURSOR LOCAL FAST_FORWARD FOR
            SELECT Id, QuantityRemaining, UnitCost, PurchaseDate, OriginalInventoryBatchId
            FROM finance.InventoryBatch
            WHERE ProductId = @ProductId
              AND ProductTypeId = @ProductTypeId
              AND LocationId = @FromLocationId
              AND QuantityRemaining > 0
              AND InventoryBatchStatusId = 2 -- Active
              AND Deleted = 0
            ORDER BY PurchaseDate ASC, Id ASC;

        OPEN curBatches;
        FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty, @UnitCost, @PurchaseDate, @OriginalInventoryBatchId;

        WHILE @@FETCH_STATUS = 0 AND @QtyToMove > 0
        BEGIN
            SET @DeductNow = CASE WHEN @BatchQty >= @QtyToMove THEN @QtyToMove ELSE @BatchQty END;

            -- Deduct stock
            UPDATE finance.InventoryBatch
            SET InventoryBatchStatusId = CASE WHEN @QtyToMove < @BatchQty THEN 2 /* Active */ ELSE 3 /* Depleted */ END,
                QuantityRemaining = QuantityRemaining - @DeductNow,
                AmendUserId = @CurrentUserId,
                AmendDate = @UpdateDate
            WHERE Id = @BatchId;

            -- Record activity
            INSERT INTO finance.InventoryBatchActivity (InventoryBatchId, ActivityId, Quantity, CreateUserId, CreateDate, AmendUserId, AmendDate)
            VALUES (@BatchId, @ActivityId, @DeductNow, @CurrentUserId, @UpdateDate, @CurrentUserId, @UpdateDate);

            -- Add stock to new location
            INSERT INTO finance.InventoryBatch (InventoryBatchStatusId, ProductId, ProductTypeId, LocationId, InitialQuantity, QuantityRemaining, UnitCost, PurchaseDate, OriginalInventoryBatchId, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
            VALUES (2 /* Active */, @ProductId, @ProductTypeId, @ToLocationId, @DeductNow, @DeductNow, @UnitCost, @PurchaseDate, @OriginalInventoryBatchId, 0, @CurrentUserId, @UpdateDate, @CurrentUserId, @UpdateDate);

            -- Record activity for new batch
            DECLARE @NewBatchId INT = SCOPE_IDENTITY();
            INSERT INTO finance.InventoryBatchActivity (InventoryBatchId, ActivityId, Quantity, CreateUserId, CreateDate, AmendUserId, AmendDate)
            VALUES (@NewBatchId, @ActivityId, @DeductNow, @CurrentUserId, @UpdateDate, @CurrentUserId, @UpdateDate);

            SET @QtyToMove = @QtyToMove - @DeductNow;

            FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty, @UnitCost, @PurchaseDate, @OriginalInventoryBatchId;
        END

        CLOSE curBatches;
        DEALLOCATE curBatches;

        IF @QtyToMove > 0
        BEGIN
            RAISERROR('Not enough stock to fulfil the move request. ActivityId=%d', 16, 1, @ActivityId);
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
		
		INSERT INTO dbo.ErrorLog (ErrorDate,	ProcedureName, ErrorNumber, ErrorSeverity, ErrorState, ErrorLine, ErrorMessage, UserId)
		VALUES (GETDATE(), ERROR_PROCEDURE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_MESSAGE(), @CurrentUserId);

        -- Re-raise the error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END

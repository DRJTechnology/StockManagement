-- =========================================================================
-- Author:		Dave Brown
-- Create date: 10 Aug 2025
-- Description:	Cteate Inventory Batch
-- =========================================================================
CREATE PROCEDURE [finance].[InventoryBatch_Create]
    @ProductId INT,
    @ProductTypeId INT,
    @LocationId INT,
    @Quantity INT,
    @UnitCost MONEY,
    @ActivityDate DATETIME,
    @ActivityId INT,
    @UserId INT
AS

    DECLARE @NewInventoryBatchId INT,
            @AmendDate DATETIME = GETDATE()

    INSERT INTO finance.InventoryBatch (ProductId,ProductTypeId,LocationId,InitialQuantity,QuantityRemaining,UnitCost,PurchaseDate,Deleted,CreateUserId,CreateDate,AmendUserId,AmendDate)
    VALUES (
        @ProductId,
        @ProductTypeId,
        @LocationId,
        @Quantity,
        @Quantity,
        @UnitCost,
        @ActivityDate,
        0, -- Deleted
        @UserId, -- CreateUserId
        GETDATE(),
        @UserId, -- AmendUserId
        @AmendDate
    )

    -- Get the newly created InventoryBatch Id
    SET @NewInventoryBatchId = SCOPE_IDENTITY()

    -- Insert into InventoryBatchActivity
    INSERT INTO finance.InventoryBatchActivity (InventoryBatchId,ActivityId,Quantity,Deleted,CreateUserId,CreateDate,AmendUserId,AmendDate)
    VALUES (
        @NewInventoryBatchId,
        @ActivityId,
        @Quantity,
        0, -- Deleted
        1, -- CreateUserId
        GETDATE(),
        1, -- AmendUserId
        @AmendDate
    )

RETURN 0

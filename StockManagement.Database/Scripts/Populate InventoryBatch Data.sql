
-- Cursor to iterate over Activity records
DECLARE activity_cursor CURSOR FOR
SELECT Id, ActivityDate, ActionId, LocationId, ProductId, ProductTypeId, Quantity, AmendDate
FROM Activity
WHERE Deleted = 0
ORDER BY ActivityDate, ActionId

DECLARE @Id INT,
        @ActivityDate DATETIME,
        @ActionId INT,
        @LocationId INT,
        @ProductId INT,
        @ProductTypeId INT,
        @Quantity INT,
        @AmendDate DATETIME,
        @NewInventoryBatchId INT

OPEN activity_cursor
FETCH NEXT FROM activity_cursor INTO @Id, @ActivityDate, @ActionId, @LocationId, @ProductId, @ProductTypeId, @Quantity, @AmendDate

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @ActionId = 1 OR @ActionId = 2 -- Add new stock add to OR Move from stock room to
    BEGIN
        INSERT INTO finance.InventoryBatch (ProductId,ProductTypeId,LocationId,QuantityPurchased,QuantityRemaining,UnitCost,PurchaseDate,Deleted,CreateUserId,CreateDate,AmendUserId,AmendDate)
        VALUES (
            @ProductId,
            @ProductTypeId,
            @LocationId,
            @Quantity,
            @Quantity,
            0.34, -- Set UnitCost, adjustment needed
            @ActivityDate,
            0, -- Deleted
            1, -- CreateUserId
            GETDATE(),
            1, -- AmendUserId
            @AmendDate
        )

        -- Get the newly created InventoryBatch Id
        SET @NewInventoryBatchId = SCOPE_IDENTITY()

        -- Insert into InventoryBatchActivity
        INSERT INTO finance.InventoryBatchActivity (InventoryBatchId,ActivityId,Deleted,CreateUserId,CreateDate,AmendUserId,AmendDate)
        VALUES (
            @NewInventoryBatchId,
            @Id,
            0, -- Deleted
            1, -- CreateUserId
            GETDATE(),
            1, -- AmendUserId
            @AmendDate
        )
    END
    IF  @ActionId = 2 -- Move from stock room to
    BEGIN
        -- Remove From Stock room

    END

    FETCH NEXT FROM activity_cursor INTO @Id, @ActivityDate, @ActionId, @LocationId, @ProductId, @ProductTypeId, @Quantity, @AmendDate
END

CLOSE activity_cursor
DEALLOCATE activity_cursor

-- Optionally, show results
SELECT * FROM finance.InventoryBatch
SELECT * FROM finance.InventoryBatchActivity

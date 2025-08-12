
-- Cursor to iterate over Activity records
DECLARE activity_cursor CURSOR FOR
SELECT Id, ActivityDate, ActionId, LocationId, ProductId, ProductTypeId, Quantity, AmendDate
FROM Activity
WHERE Deleted = 0
AND ActionId = 1 -- Only include adding new stock
--AND ActionId != 1 -- Include all except adding new stock
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
    IF @ActionId = 1 -- Add new stock add to
    BEGIN
        EXEC	[finance].[InventoryBatch_Create]
		@ProductId = @ProductId,
		@ProductTypeId = @ProductTypeId,
		@LocationId = @LocationId,
		@Quantity = @Quantity,
		@ActivityDate = @ActivityDate,
		@ActivityId = @Id,
		@UserId = 1

    END
    ELSE IF  @ActionId = 2 -- Move from stock room to
    BEGIN
        EXEC	[finance].[InventoryBatch_Create]
		@ProductId = @ProductId,
		@ProductTypeId = @ProductTypeId,
		@LocationId = @LocationId,
		@Quantity = @Quantity,
		@ActivityDate = @ActivityDate,
		@ActivityId = @Id,
		@UserId = 1

        -- Remove From Stock room
        EXEC	[finance].[InventoryBatch_ReduceQuantity]
		@ProductId = @ProductId,
		@ProductTypeId = @ProductTypeId,
		@LocationId = 1,
		@Quantity = @Quantity,
		@ActivityId = @Id,
		@UserId = 1
    END
    ELSE IF @ActionId = 3 -- Return to stock room from
    BEGIN
        -- Remove From location
        EXEC	[finance].[InventoryBatch_ReduceQuantity]
		@ProductId = @ProductId,
		@ProductTypeId = @ProductTypeId,
		@LocationId = @LocationId,
		@Quantity = @Quantity,
		@ActivityId = @Id,
		@UserId = 1

        -- Add to Stock room
        EXEC	[finance].[InventoryBatch_Create]
        @ProductId = @ProductId,
        @ProductTypeId = @ProductTypeId,
        @LocationId = 1, -- stock room
        @Quantity = @Quantity,
        @ActivityDate = @ActivityDate,
        @ActivityId = @Id,
        @UserId = 1
    END
    ELSE IF @ActionId = 4 -- Delete stock
    BEGIN
        -- Remove Stock
        EXEC	[finance].[InventoryBatch_ReduceQuantity]
        @ProductId = @ProductId,
        @ProductTypeId = @ProductTypeId,
        @LocationId = @LocationId,
        @Quantity = @Quantity,
        @ActivityId = @Id,
        @UserId = 1
    END
    ELSE IF @ActionId = 5 -- Sale of stock
    BEGIN
        -- Remove Stock
        EXEC	[finance].[InventoryBatch_ReduceQuantity]
        @ProductId = @ProductId,
        @ProductTypeId = @ProductTypeId,
        @LocationId = @LocationId,
        @Quantity = @Quantity,
        @ActivityId = @Id,
        @UserId = 1
    END
    ELSE IF @ActionId = 6 -- Promotional use
    BEGIN
        -- Remove Stock
        EXEC	[finance].[InventoryBatch_ReduceQuantity]
        @ProductId = @ProductId,
        @ProductTypeId = @ProductTypeId,
        @LocationId = @LocationId,
        @Quantity = @Quantity,
        @ActivityId = @Id,
        @UserId = 1
    END
    ELSE IF @ActionId = 7 -- Damaged Stock
    BEGIN
        -- Remove Stock
        EXEC	[finance].[InventoryBatch_ReduceQuantity]
        @ProductId = @ProductId,
        @ProductTypeId = @ProductTypeId,
        @LocationId = @LocationId,
        @Quantity = @Quantity,
        @ActivityId = @Id,
        @UserId = 1
    END

    FETCH NEXT FROM activity_cursor INTO @Id, @ActivityDate, @ActionId, @LocationId, @ProductId, @ProductTypeId, @Quantity, @AmendDate
END

CLOSE activity_cursor
DEALLOCATE activity_cursor

-- Optionally, show results
SELECT * FROM finance.InventoryBatch
SELECT * FROM finance.InventoryBatchActivity

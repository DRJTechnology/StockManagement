
-- Cursor to iterate over Activity records
DECLARE activity_cursor CURSOR FOR
SELECT Id, ActivityDate, ActionId, LocationId, ProductId, ProductTypeId, Quantity, AmendDate
FROM Activity
WHERE Deleted = 0
AND ActionId = 1 -- Only include adding new stock
--AND ActionId != 1 -- Include all except adding new stock
AND ProductTypeId != 1 -- Exclude Originals
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
        @UnitCost = 0,
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
        @UnitCost = 0,
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
        @UnitCost = 0,
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
GO


--Update unit price based on each order
UPDATE finance.InventoryBatch SET UnitCost = 31.55/20 WHERE PurchaseDate = '14 Apr 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = 63.15/80 WHERE PurchaseDate = '23 Apr 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = 81.95/100 WHERE PurchaseDate = '23 May 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = ??/100 WHERE PurchaseDate = '30 May 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = 126.95/200 WHERE PurchaseDate = '10 Jun 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = 227.95/500 WHERE PurchaseDate = '14 Jul 2025' AND ProductTypeId = 2 -- Card

UPDATE finance.InventoryBatch SET UnitCost = 62.80/12 WHERE PurchaseDate = '23 May 2025' AND ProductTypeId = 5 -- Print A4
UPDATE finance.InventoryBatch SET UnitCost = 39.60/12 WHERE PurchaseDate = '23 May 2025' AND ProductTypeId = 3 -- Print A6


-- Optionally, show results
SELECT * FROM finance.InventoryBatch
SELECT * FROM finance.InventoryBatchActivity

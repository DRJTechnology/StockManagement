
-- Cursor to iterate over Activity records
DECLARE activity_cursor CURSOR FOR
SELECT Id, ActivityDate, ActionId, LocationId, ProductId, ProductTypeId, Quantity, AmendDate
FROM Activity
WHERE Deleted = 0
AND ActionId = 1 -- Only include adding new stock
--AND ActionId != 1 -- Include all except adding new stock
AND ProductTypeId != 1 -- Exclude Originals
ORDER BY ActivityDate, Id --, ActionId

DECLARE @Id INT,
        @ActivityDate DATETIME,
        @ActionId INT,
        @LocationId INT,
        @ProductId INT,
        @ProductTypeId INT,
        @Quantity INT,
        @AmendDate DATETIME,
        @NewInventoryBatchId INT,
		@Success BIT,
		@TempId INT

OPEN activity_cursor
FETCH NEXT FROM activity_cursor INTO @Id, @ActivityDate, @ActionId, @LocationId, @ProductId, @ProductTypeId, @Quantity, @AmendDate

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @ActionId = 1 -- Add new stock add to
    BEGIN
        EXEC	[finance].[InventoryBatch_Create]
        @Success = @Success OUTPUT,
        @Id = @TempId OUTPUT,
        @InventoryBatchStatusId = 2, -- Active
		@ProductId = @ProductId,
		@ProductTypeId = @ProductTypeId,
		@LocationId = @LocationId,
		@InitialQuantity = @Quantity,
        @UnitCost = 0,
		@ActivityDate = @ActivityDate,
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
UPDATE finance.InventoryBatch SET UnitCost = 63.15/100 WHERE PurchaseDate = '23 Apr 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = 81.95/100 WHERE PurchaseDate = '30 May 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = 126.95/200 WHERE PurchaseDate = '10 Jun 2025' AND ProductTypeId = 2 -- Card
UPDATE finance.InventoryBatch SET UnitCost = 227.95/500 WHERE PurchaseDate = '14 Jul 2025' AND ProductTypeId = 2 -- Card

UPDATE finance.InventoryBatch SET UnitCost = 62.80/12 WHERE PurchaseDate = '23 May 2025' AND ProductTypeId = 5 -- Print A4
UPDATE finance.InventoryBatch SET UnitCost = 39.60/12 WHERE PurchaseDate = '23 May 2025' AND ProductTypeId = 3 -- Print A6

UPDATE finance.InventoryBatch SET UnitCost = 11.00/1 WHERE PurchaseDate = '14 Jun 2025' AND ProductTypeId = 6 -- Print 30x30cm

UPDATE finance.InventoryBatch SET UnitCost = 4.23 WHERE PurchaseDate = '11 Jun 2025' AND ProductTypeId = 3 -- Print A6
UPDATE finance.InventoryBatch SET UnitCost = 4.23 WHERE PurchaseDate = '21 Jun 2025' AND ProductTypeId = 3 -- Print A6

UPDATE finance.InventoryBatch SET UnitCost = 3.70 WHERE PurchaseDate = '21 Jun 2025' AND ProductTypeId = 4 --(Qty 7) Print A5
UPDATE finance.InventoryBatch SET UnitCost = 6.48 WHERE PurchaseDate = '24 Jun 2025' AND ProductTypeId = 4 -- Print A5
UPDATE finance.InventoryBatch SET UnitCost = 3.70 WHERE PurchaseDate = '08 Aug 2025' AND ProductTypeId = 4 --(Qty 2) Print A5
UPDATE finance.InventoryBatch SET UnitCost = 6.48 WHERE PurchaseDate = '23 Apr 2025' AND ProductTypeId = 4 AND ProductId = 23 -- Print A5 Whispers amongst petals 
UPDATE finance.InventoryBatch SET UnitCost = 6.48 WHERE PurchaseDate = '23 Apr 2025' AND ProductTypeId = 4 AND ProductId in (26,8) -- Print A5 (Meadow Blooms,Day Dreams)
UPDATE finance.InventoryBatch SET UnitCost = 6.48 WHERE PurchaseDate = '23 Apr 2025' AND ProductTypeId = 4 AND ProductId = 14 -- Print A5 Lunar serenity
GO


-- Cursor to iterate over Activity records
DECLARE activity_cursor CURSOR FOR
SELECT Id, ActivityDate, ActionId, LocationId, ProductId, ProductTypeId, Quantity, AmendDate
FROM Activity
WHERE Deleted = 0
--AND ActionId = 1 -- Only include adding new stock
AND ActionId != 1 -- Include all except adding new stock
AND ProductTypeId != 1 -- Exclude Originals
ORDER BY ActivityDate, Id --, ActionId

DECLARE @Id INT,
        @ActivityDate DATETIME,
        @ActionId INT,
        @LocationId INT,
        @ProductId INT,
        @ProductTypeId INT,
        @Quantity INT,
        @AmendDate DATETIME,
        @NewInventoryBatchId INT,
        @CostRemoved MONEY,
        @TransactionId INT,
		@Success BIT,
		@TempId INT

OPEN activity_cursor
FETCH NEXT FROM activity_cursor INTO @Id, @ActivityDate, @ActionId, @LocationId, @ProductId, @ProductTypeId, @Quantity, @AmendDate

WHILE @@FETCH_STATUS = 0
BEGIN
  --  IF @ActionId = 1 -- Add new stock add to
  --  BEGIN
  --      EXEC	[finance].[InventoryBatch_Create]
		--@ProductId = @ProductId,
		--@ProductTypeId = @ProductTypeId,
		--@LocationId = @LocationId,
		--@InitialQuantity = @Quantity,
  --      @UnitCost = 0,
		--@ActivityDate = @ActivityDate,
		--@ActivityId = @Id,
		--@UserId = 1

  --  END
  --  ELSE 
    IF  @ActionId = 2 -- Move from stock room to
    BEGIN
        EXEC	[finance].[InventoryBatch_MoveQuantity]
		@ProductId = @ProductId,
		@ProductTypeId = @ProductTypeId,
		@FromLocationId = 1, -- Stock room
		@ToLocationId = @LocationId,
		@Quantity = @Quantity,
		@ActivityId = @Id,
		@UserId = 1


  --      EXEC	[finance].[InventoryBatch_Create]
  --      @Success = @Success OUTPUT,
  --      @Id = @TempId OUTPUT,
  --      @InventoryBatchStatusId = 2, -- Active
		--@ProductId = @ProductId,
		--@ProductTypeId = @ProductTypeId,
		--@LocationId = @LocationId,
		--@InitialQuantity = @Quantity,
  --      @UnitCost = 0,
		--@ActivityDate = @ActivityDate,
		--@ActivityId = @Id,
		--@UserId = 1

  --      -- Remove From Stock room
  --      EXEC	[finance].[InventoryBatch_ReduceQuantity]
		--@ProductId = @ProductId,
		--@ProductTypeId = @ProductTypeId,
		--@LocationId = 1,
		--@Quantity = @Quantity,
		--@ActivityId = @Id,
		--@UserId = 1,
  --      @CostRemoved = @CostRemoved OUTPUT
    END
    ELSE IF @ActionId = 3 -- Return to stock room from
    BEGIN
        EXEC	[finance].[InventoryBatch_MoveQuantity]
		@ProductId = @ProductId,
		@ProductTypeId = @ProductTypeId,
		@FromLocationId = @LocationId,
		@ToLocationId = 1, -- Stock room
		@Quantity = @Quantity,
		@ActivityId = @Id,
		@UserId = 1


  --      -- Remove From location
  --      EXEC	[finance].[InventoryBatch_ReduceQuantity]
		--@ProductId = @ProductId,
		--@ProductTypeId = @ProductTypeId,
		--@LocationId = @LocationId,
		--@Quantity = @Quantity,
		--@ActivityId = @Id,
		--@UserId = 1,
  --      @CostRemoved = @CostRemoved OUTPUT

  --      -- Add to Stock room
  --      EXEC	[finance].[InventoryBatch_Create]
  --      @Success = @Success OUTPUT,
  --      @Id = @TempId OUTPUT,
  --      @InventoryBatchStatusId = 2, -- Active
  --      @ProductId = @ProductId,
  --      @ProductTypeId = @ProductTypeId,
  --      @LocationId = 1, -- stock room
  --      @InitialQuantity = @Quantity,
  --      @UnitCost = 0,
  --      @ActivityDate = @ActivityDate,
  --      @ActivityId = @Id,
  --      @UserId = 1
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
        @UserId = 1,
        @CostRemoved = @CostRemoved OUTPUT
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
        @UserId = 1,
        @CostRemoved = @CostRemoved OUTPUT

        -- TODO Create transaction for sale of stock
--        Debit (expense): Cost of Goods Sold by @CostRemoved
--       Credit (reduce asset): Inventory by @CostRemoved
		IF(@CostRemoved > 0)
		BEGIN
			INSERT INTO [finance].[Transaction] (TransactionTypeId, [Date], Reference, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
			VALUES (4, @ActivityDate, 'COGS-INV', 0, 1, SYSDATETIME(), 1, SYSDATETIME())

			SELECT @TransactionId = SCOPE_IDENTITY()

			INSERT INTO [finance].[TransactionDetail] (TransactionId, AccountId, [Date], [Description], Amount, Direction, ContactId, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
			VALUES (@TransactionId, 9 /* Cost of Goods Sold */, @ActivityDate, 'Sale of stock', @CostRemoved, 1, 3 /* General public */, 0, 1, SYSDATETIME(), 1, SYSDATETIME())
			SELECT @Id = SCOPE_IDENTITY()

			INSERT INTO [finance].[TransactionDetail] (TransactionId, AccountId, [Date], [Description], Amount, Direction, ContactId, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
			VALUES (@TransactionId, 6 /* Inventory */, @ActivityDate, 'Sale of stock', @CostRemoved, -1, 3 /* General public */, 0, 1, SYSDATETIME(), 1, SYSDATETIME())
		END

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
        @UserId = 1,
        @CostRemoved = @CostRemoved OUTPUT
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
        @UserId = 1,
        @CostRemoved = @CostRemoved OUTPUT
    END

    FETCH NEXT FROM activity_cursor INTO @Id, @ActivityDate, @ActionId, @LocationId, @ProductId, @ProductTypeId, @Quantity, @AmendDate
END

CLOSE activity_cursor
DEALLOCATE activity_cursor
GO





---- Optionally, show results
--SELECT * FROM finance.InventoryBatch
--SELECT * FROM finance.InventoryBatchActivity


---- DEBUGGING --

--SELECT * FROM [StockManagement].[finance].[InventoryBatch] where ProductTypeId = 2
--SELECT * FROM [StockManagement].[finance].[InventoryBatchActivity]
--where InventoryBatchId = 1002

----delete from [finance].[InventoryBatchActivity]
----delete from [finance].[InventoryBatch]

--select * from Activity


--SELECT top 5 p.ProductName, ib.* FROM [finance].[InventoryBatch] ib
--INNER JOIN Product p on ib.ProductId = p.Id
--where ProductTypeId = 2

--SELECT a.ActivityDate, a.Quantity AS QuantityRequired, iba.Quantity AS QuantityAffected, a.ActionId, act.ActionName, a.LocationId, l.[Name] AS LocationName, iba.*
--FROM [finance].[InventoryBatchActivity] iba
--INNER JOIN Activity a ON iba.ActivityId = a.Id
--INNER JOIN [Action] act on a.ActionId = act.Id
--INNER JOIN [Location] l on a.LocationId = l.Id
--where InventoryBatchId = 1003
--	AND a.Deleted = 0

--select * from finance.InventoryBatch
--Where ProductId = 5 AND ProductTypeId = 2

---- Activities with insufficient stock
--SELECT	a.Id, a.ActivityDate, a.Quantity, a.ActionId, atn.ActionName, l.[Name] AS LocationName, pt.ProductTypeName, p.ProductName, a.ProductId, a.ProductTypeId, a.LocationId
--FROM	Activity a
--INNER JOIN [Action] atn ON a.ActionId = atn.Id
--INNER JOIN Product p ON a.ProductId = p.Id
--INNER JOIN ProductType pt ON a.ProductTypeId = pt.Id
--INNER JOIN Location l ON a.LocationId = l.Id
--WHERE a.Id in (1187,1188,1194,1198,1201,3359) -- ActivityIds where there is insuficient stock
----WHERE a.Id in (1187,1188,1194,1198,1201,3359) -- ActivityIds where there is insuficient stock


---- InventoryBatch records with zero unit cost       
--SELECT ib.Id, ib.PurchaseDate, ib.ProductId, p.ProductName, ib.ProductTypeId, pt.ProductTypeName, l.Name AS LocationName, ib.InitialQuantity, ib.QuantityRemaining, iba.ActivityId, atn.ActionName
--from finance.InventoryBatch ib
--INNER JOIN finance.InventoryBatchActivity iba on ib.Id = iba.InventoryBatchId
--INNER JOIN Product p ON ib.ProductId = p.Id
--INNER JOIN ProductType pt ON ib.ProductTypeId = pt.Id
--INNER JOIN Location l ON ib.LocationId = l.Id
--INNER JOIN Activity a ON iba.ActivityId = a.Id
--INNER JOIN [Action] atn ON a.ActionId = atn.Id
--where ib.UnitCost = 0
---- 24 rows


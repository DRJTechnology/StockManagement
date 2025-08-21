---- =========================================================================
---- Author:		Dave Brown
---- Create date: 19 Aug 2025
---- Description:	Sell Stock
---- =========================================================================
--CREATE PROCEDURE [finance].[Inventory_SaleOfStock]
--    @ProductId INT,
--    @ProductTypeId INT,
--    @LocationId INT,
--    @Quantity INT,
--    @ContactId INT = 3, -- General public
--    @SalePrice MONEY,
--    @SaleDate DATETIME,
--    @ActivityId INT,
--    @UserId INT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    -- Accounts
--    DECLARE @OwnersAccountId INT = 3    -- Owner’s Investment/Drawings
--    DECLARE @SalesAccountId INT = 9     -- Sales - Art
--    DECLARE @CogsAccountId INT = 8      -- Cost of Goods Sold
--    DECLARE @InventoryAccountId INT = 5 -- Inventory 

--    DECLARE	@CostRemoved money,
--        	@Success bit,
--		    @TransactionDetailId int

--    -- Reduce Inventory Batch Quantity
--    EXEC	[finance].[InventoryBatch_ReduceQuantity]
--		    @ProductId = @ProductId,
--		    @ProductTypeId = @ProductTypeId,
--		    @LocationId = @LocationId,
--		    @Quantity = @Quantity,
--		    @ActivityId = @ActivityId,
--		    @UserId = @UserId,
--		    @CostRemoved = @CostRemoved OUTPUT

--    -- Create the sale transaction
--    EXEC	@return_value = [finance].[Transaction_CreateExpenseIncome]
--		    @Success = @Success OUTPUT,
--		    @Id = @TransactionDetailId OUTPUT,
--		    @TransactionTypeId = 3, -- Income
--		    @AccountId = @SalesAccountId,
--		    @Date = @SaleDate,
--		    @Description = N'Sale of stock',
--		    @Amount = @SalePrice,
--		    @ContactId = @ContactId,
--		    @CurrentUserId = @UserId

--    -- Record cost of goods sold transaction
--    --Debit (expense): Cost of Goods Sold e.g. £15
--    --Credit (reduce asset): Inventory e.g. £15



--    --SELECT	@Success as N'@Success',
--		  --  @TransactionDetailId as N'@Id'

--    --SELECT	@CostRemoved as N'@CostRemoved'


--    --DECLARE @QtyToDeduct INT = @Quantity;
--    --DECLARE @BatchId INT;
--    --DECLARE @BatchQty INT;
--    --DECLARE @DeductNow INT;

--    --DECLARE curBatches CURSOR LOCAL FAST_FORWARD FOR
--    --    SELECT Id, QuantityRemaining
--    --    FROM finance.InventoryBatch
--    --    WHERE ProductId = @ProductId
--    --      AND ProductTypeId = @ProductTypeId
--    --      AND LocationId = @LocationId
--    --      AND QuantityRemaining > 0
--    --      AND Deleted = 0
--    --    ORDER BY PurchaseDate ASC, Id ASC;

--    --OPEN curBatches;
--    --FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty;

--    --WHILE @@FETCH_STATUS = 0 AND @QtyToDeduct > 0
--    --BEGIN
--    --    SET @DeductNow = CASE WHEN @BatchQty >= @QtyToDeduct THEN @QtyToDeduct ELSE @BatchQty END;

--    --    -- Deduct stock
--    --    UPDATE finance.InventoryBatch
--    --    SET QuantityRemaining = QuantityRemaining - @DeductNow,
--    --        AmendUserId = @UserId,
--    --        AmendDate = GETDATE()
--    --    WHERE Id = @BatchId;

--    --    -- Record activity
--    --    INSERT INTO finance.InventoryBatchActivity (InventoryBatchId, ActivityId, Quantity, CreateUserId, AmendUserId)
--    --    VALUES (@BatchId, @ActivityId, @DeductNow, @UserId, @UserId);

--    --    SET @QtyToDeduct = @QtyToDeduct - @DeductNow;

--    --    FETCH NEXT FROM curBatches INTO @BatchId, @BatchQty;
--    --END

--    --CLOSE curBatches;
--    --DEALLOCATE curBatches;

--    --IF @QtyToDeduct > 0
--    --BEGIN
--    --    RAISERROR('Not enough stock to fulfil request. ActivityId=%d', 16, 1, @ActivityId);
--    --END
--END
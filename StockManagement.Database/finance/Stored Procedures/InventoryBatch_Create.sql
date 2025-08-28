-- =========================================================================
-- Author:		Dave Brown
-- Create date: 10 Aug 2025
-- Description:	Create Inventory Batch
-- =========================================================================
CREATE PROCEDURE [finance].[InventoryBatch_Create]
	@Success    BIT OUTPUT,
	@Id         INT OUTPUT, 
    @InventoryBatchStatusId SMALLINT,
    @ProductId      INT,
    @ProductTypeId  INT,
    @LocationId     INT,
    @InitialQuantity INT,
    @UnitCost       MONEY,
    @ActivityDate   DATETIME,
    @ActivityId     INT,
    @UserId         INT
AS
BEGIN
	SET NOCOUNT OFF
	DECLARE @Err int

    DECLARE @AmendDate DATETIME = GETDATE()

    INSERT INTO finance.InventoryBatch (InventoryBatchStatusId, ProductId,ProductTypeId,LocationId,InitialQuantity,QuantityRemaining,UnitCost,PurchaseDate,Deleted,CreateUserId,CreateDate,AmendUserId,AmendDate)
    VALUES (
        @InventoryBatchStatusId,
        @ProductId,
        @ProductTypeId,
        @LocationId,
        @InitialQuantity,
        @InitialQuantity,
        @UnitCost,
        @ActivityDate,
        0, -- Deleted
        @UserId, -- CreateUserId
        @AmendDate,
        @UserId, -- AmendUserId
        @AmendDate
    )

    -- Get the newly created InventoryBatch Id
    SET @Id = SCOPE_IDENTITY()

    -- Insert into InventoryBatchActivity
    INSERT INTO finance.InventoryBatchActivity (InventoryBatchId,ActivityId,Quantity,Deleted,CreateUserId,CreateDate,AmendUserId,AmendDate)
    VALUES (
        @Id,
        @ActivityId,
        @InitialQuantity,
        0, -- Deleted
        1, -- CreateUserId
        @AmendDate,
        1, -- AmendUserId
        @AmendDate
    )

	RETURN @Err
END

CREATE TABLE [finance].[InventoryBatch]
(
	[Id]            INT IDENTITY (1, 1) NOT NULL,
    [InventoryBatchStatusId]    SMALLINT NOT NULL,
    [ProductId]     INT         NOT NULL, 
    [ProductTypeId] INT         NOT NULL, 
    [LocationId]    INT         NOT NULL, 
    [InitialQuantity]   INT     NOT NULL, 
    [QuantityRemaining] INT     NOT NULL, 
    [UnitCost]      MONEY       NOT NULL, 
    [PurchaseDate]  DATETIME    NOT NULL,
    [OriginalInventoryBatchId] INT NOT NULL,
    [Deleted]       BIT         DEFAULT ((0)) NOT NULL,
    [CreateUserId]  INT         NOT NULL,
    [CreateDate]    DATETIME    CONSTRAINT [DF_InventoryBatch_CreateDate] DEFAULT (getdate()) NOT NULL,
    [AmendUserId]   INT         NOT NULL,
    [AmendDate]     DATETIME    CONSTRAINT [DF_InventoryBatch_AmendDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_InventoryBatch] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_InventoryBatch_InventoryBatchStatus] FOREIGN KEY ([InventoryBatchStatusId]) REFERENCES [finance].[InventoryBatchStatus]([Id]), 
    CONSTRAINT [FK_InventoryBatch_Product] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id]), 
    CONSTRAINT [FK_InventoryBatch_ProductType] FOREIGN KEY ([ProductTypeId]) REFERENCES [ProductType]([Id]), 
    CONSTRAINT [FK_InventoryBatch_Location] FOREIGN KEY ([LocationId]) REFERENCES [Location]([Id])
);


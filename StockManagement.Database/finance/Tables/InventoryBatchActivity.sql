CREATE TABLE [finance].[InventoryBatchActivity]
(
	[Id]                INT IDENTITY (1, 1) NOT NULL,
    [InventoryBatchId]  INT         NOT NULL, 
    [ActivityId]        INT         NOT NULL, 
    [Quantity]          INT         NOT NULL, 
    [Deleted]           BIT         DEFAULT ((0)) NOT NULL,
    [CreateUserId]      INT         NOT NULL,
    [CreateDate]        DATETIME    CONSTRAINT [DF_InventoryBatchAction_CreateDate] DEFAULT (getdate()) NOT NULL,
    [AmendUserId]       INT         NOT NULL,
    [AmendDate]         DATETIME    CONSTRAINT [DF_InventoryBatchAction_AmendDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_InventoryBatchActivity] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_InventoryBatchActivity_InventoryBatch] FOREIGN KEY ([InventoryBatchId]) REFERENCES [finance].[InventoryBatch]([Id]), 
    CONSTRAINT [FK_InventoryBatchActivity_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [Activity]([Id])
);


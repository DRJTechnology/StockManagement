CREATE TABLE [finance].[InventoryBatchStatus] (
    [Id]           SMALLINT       NOT NULL,
    [Status]       NVARCHAR (255) NOT NULL,
    [Deleted]      BIT            NOT NULL,
    [CreateUserId] INT            NOT NULL,
    [CreateDate]   DATETIME       NOT NULL,
    [AmendUserId]  INT            NOT NULL,
    [AmendDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_InventoryBatchStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);


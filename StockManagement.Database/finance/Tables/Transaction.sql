CREATE TABLE [finance].[Transaction] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [TransactionTypeId] SMALLINT       NOT NULL,
    [Date]              DATETIME       NOT NULL,
    [Reference]         NVARCHAR (256) NOT NULL,
    [Deleted]           BIT            NOT NULL,
    [CreateUserId]      INT            NOT NULL,
    [CreateDate]        DATETIME       NOT NULL,
    [AmendUserId]       INT            NOT NULL,
    [AmendDate]         DATETIME       NOT NULL,
    CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Transaction_TransactionType] FOREIGN KEY ([TransactionTypeId]) REFERENCES [finance].[TransactionType]([Id])
);


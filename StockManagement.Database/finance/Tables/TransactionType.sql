CREATE TABLE [finance].[TransactionType] (
    [Id]           SMALLINT       NOT NULL,
    [Type]         NVARCHAR (255) NOT NULL,
    [Deleted]      BIT            NOT NULL,
    [CreateUserId] INT            NOT NULL,
    [CreateDate]   DATETIME       NOT NULL,
    [AmendUserId]  INT            NOT NULL,
    [AmendDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_TransactionType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


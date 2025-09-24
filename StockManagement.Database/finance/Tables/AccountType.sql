CREATE TABLE [finance].[AccountType] (
    [Id]           SMALLINT       NOT NULL,
    [Type]         NVARCHAR (255) NOT NULL,
    [CreditDebit]  SMALLINT       NOT NULL,
    [Deleted]      BIT            NOT NULL,
    [CreateUserId] INT            NOT NULL,
    [CreateDate]   DATETIME       NOT NULL,
    [AmendUserId]  INT            NOT NULL,
    [AmendDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


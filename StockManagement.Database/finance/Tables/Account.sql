CREATE TABLE [finance].[Account] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [AccountTypeId] SMALLINT        NOT NULL,
    [Name]          NVARCHAR (255)  NOT NULL,
    [Notes]         NVARCHAR (4000) NULL,
    [Active]        BIT             NOT NULL,
    [Deleted]       BIT             NOT NULL,
    [CreateUserId]  INT             NOT NULL,
    [CreateDate]    DATETIME        NOT NULL,
    [AmendUserId]   INT             NOT NULL,
    [AmendDate]     DATETIME        NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Account_AccountType] FOREIGN KEY ([AccountTypeId]) REFERENCES [finance].[AccountType]([Id])
);


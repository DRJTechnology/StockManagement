CREATE TABLE [dbo].[StockReceipt] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME NOT NULL,
    [ContactId]   INT      NOT NULL,
    [Deleted]     BIT      CONSTRAINT [DF_StockReceipt_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT      NOT NULL,
    [AmendDate]   DATETIME CONSTRAINT [DF_StockReceipt_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockReceipt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockReceipt_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([Id])
);


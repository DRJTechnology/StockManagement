CREATE TABLE [dbo].[StockReceipt] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME NOT NULL,
    [SupplierId]  INT      NOT NULL,
    [Deleted]     BIT      CONSTRAINT [DF_StockReceipt_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT      NOT NULL,
    [AmendDate]   DATETIME CONSTRAINT [DF_StockReceipt_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockReceipt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockReceipt_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Supplier] ([Id])
);


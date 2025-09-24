CREATE TABLE [dbo].[StockSale] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME NOT NULL,
    [LocationId]  INT      NOT NULL,
    [ContactId]   INT      NOT NULL,
    [SaleConfirmed]   BIT  CONSTRAINT [DF_StockSale_SaleConfirmed] DEFAULT ((0)) NOT NULL,
    [PaymentReceived] BIT  CONSTRAINT [DF_StockSale_PaymentReceived] DEFAULT ((0)) NOT NULL,
    [TotalPrice]      MONEY NULL,
    [TransactionId]   INT   NULL,
    [Deleted]     BIT      CONSTRAINT [DF_StockSale_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT      NOT NULL,
    [AmendDate]   DATETIME CONSTRAINT [DF_StockSale_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockSale] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockSale_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]),
    CONSTRAINT [FK_StockSale_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([Id]),
    CONSTRAINT [FK_StockSale_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [finance].[Transaction] ([Id])
);


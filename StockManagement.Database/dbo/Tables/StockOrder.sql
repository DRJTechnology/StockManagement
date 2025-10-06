CREATE TABLE [dbo].[StockOrder] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [Date]                  DATETIME NOT NULL,
    [ContactId]             INT      NOT NULL,
    [TotalCost]             MONEY    NULL,
    [PaymentRecorded]       BIT      CONSTRAINT [DF_StockOrder_PaymentRecorded] DEFAULT ((0)) NOT NULL,
    [StockReceiptRecorded]  BIT      CONSTRAINT [DF_StockOrder_StockOrderRecorded] DEFAULT ((0)) NOT NULL,
    [TransactionId]		    INT      NULL,
    [Deleted]               BIT      CONSTRAINT [DF_StockOrder_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]           INT      NOT NULL,
    [AmendDate]             DATETIME CONSTRAINT [DF_StockOrder_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockOrder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockOrder_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([Id]),
    CONSTRAINT [FK_StockOrder_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [finance].[Transaction] ([Id])
);


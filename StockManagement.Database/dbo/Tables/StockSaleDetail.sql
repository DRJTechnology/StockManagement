CREATE TABLE [dbo].[StockSaleDetail] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [StockSaleId] INT      NOT NULL,
    [ProductId]      INT      NOT NULL,
    [ProductTypeId]  INT      NOT NULL,
    [Quantity]       INT      NOT NULL,
    [Deleted]        BIT      CONSTRAINT [DF_StockSaleDetail_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]    INT      NOT NULL,
    [AmendDate]      DATETIME CONSTRAINT [DF_StockSaleDetail_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockSaleDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockSaleDetail_StockSale] FOREIGN KEY ([StockSaleId]) REFERENCES [dbo].[StockSale] ([Id]),
    CONSTRAINT [FK_StockSaleDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_StockSaleDetail_ProductType] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductType] ([Id])
);


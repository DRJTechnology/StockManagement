-- If the [dbo].[ProductProductType] tableexists, drop it
IF OBJECT_ID('dbo.ProductProductType', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[ProductProductType];
END
GO


-- Create the StockOrder table
CREATE TABLE [dbo].[StockOrder] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [Date]                  DATETIME NOT NULL,
    [ContactId]             INT      NOT NULL,
    [PaymentRecorded]       BIT      CONSTRAINT [DF_StockOrder_PaymentRecorded] DEFAULT ((0)) NOT NULL,
    [StockOrderRecorded]    BIT      CONSTRAINT [DF_StockOrder_StockOrderRecorded] DEFAULT ((0)) NOT NULL,
    [Deleted]               BIT      CONSTRAINT [DF_StockOrder_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]           INT      NOT NULL,
    [AmendDate]             DATETIME CONSTRAINT [DF_StockOrder_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockOrder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockOrder_Contact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([Id])
);
GO
-- Create the Stock order Detail table
CREATE TABLE [dbo].[StockOrderDetail] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [StockOrderId]   INT      NOT NULL,
    [ProductId]      INT      NOT NULL,
    [ProductTypeId]  INT      NOT NULL,
    [Quantity]       INT      NOT NULL,
    [Deleted]        BIT      CONSTRAINT [DF_StockOrderDetail_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]    INT      NOT NULL,
    [AmendDate]      DATETIME CONSTRAINT [DF_StockOrderDetail_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockOrderDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockOrderDetail_StockOrder] FOREIGN KEY ([StockOrderId]) REFERENCES [dbo].[StockOrder] ([Id]),
    CONSTRAINT [FK_StockOrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_StockOrderDetail_ProductType] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductType] ([Id])
);
GO

-- Copy the data from the StockReceipt table to the StockOrder table, maintaining the Id values
SET IDENTITY_INSERT [dbo].[StockOrder] ON;
INSERT INTO [dbo].[StockOrder] ([Id], [Date], [ContactId], [PaymentRecorded], [StockOrderRecorded], [Deleted], [AmendUserID], [AmendDate])
SELECT [Id], [Date], [ContactId], 1, 1, [Deleted], [AmendUserID], [AmendDate]
FROM [dbo].[StockReceipt]
SET IDENTITY_INSERT [dbo].[StockOrder] OFF;
GO
-- Copy the data from the StockReceiptDetail table to the StockOrderDetail table, maintaining the Id values
SET IDENTITY_INSERT [dbo].[StockOrderDetail] ON;
INSERT INTO [dbo].[StockOrderDetail] ([Id], [StockOrderId], [ProductId], [ProductTypeId], [Quantity], [Deleted], [AmendUserID], [AmendDate])
SELECT [Id], [StockReceiptId], [ProductId], [ProductTypeId], [Quantity], [Deleted], [AmendUserID], [AmendDate]
FROM [dbo].[StockReceiptDetail]
SET IDENTITY_INSERT [dbo].[StockOrderDetail] OFF;
GO

-- Rename the StockReceiptDetailId column in the dbo.Activity table to StockOrderDetailId
EXEC sp_rename 'dbo.Activity.StockReceiptDetailId', 'StockOrderDetailId', 'COLUMN';
GO

-- Drop the StockReceipt and StockReceiptDetail tables
DROP TABLE [dbo].[StockReceiptDetail];
DROP TABLE [dbo].[StockReceipt];
GO

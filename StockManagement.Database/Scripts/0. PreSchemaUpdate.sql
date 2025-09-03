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
    [StockReceiptRecorded]  BIT      CONSTRAINT [DF_StockOrder_StockOrderRecorded] DEFAULT ((0)) NOT NULL,
    [TransactionId]		    INT      NULL,
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
INSERT INTO [dbo].[StockOrder] ([Id], [Date], [ContactId], [PaymentRecorded], [StockReceiptRecorded], [Deleted], [AmendUserID], [AmendDate])
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

-- Creaate the StockSale table
CREATE TABLE [dbo].[StockSale] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME NOT NULL,
    [LocationId]  INT      NOT NULL,
    [DirectSale]  BIT      DEFAULT ((0)) NOT NULL,
    [TransactionId]  INT   NULL,
    [Deleted]     BIT      CONSTRAINT [DF_StockSale_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT      NOT NULL,
    [AmendDate]   DATETIME CONSTRAINT [DF_StockSale_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_StockSale] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockSale_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id])
);
GO

-- Create the Stock Sale Detail table
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
GO

-- Copy the data from the DeliveryNote table to the StockSale table, maintaining the Id values
SET IDENTITY_INSERT [dbo].[StockSale] ON;
INSERT INTO [dbo].[StockSale] ([Id], [Date], [LocationId], [DirectSale], [TransactionId], [Deleted], [AmendUserID], [AmendDate])
SELECT [Id], [Date], [LocationId], [DirectSale], NULL, [Deleted], [AmendUserID], [AmendDate]
FROM [dbo].[DeliveryNote]
SET IDENTITY_INSERT [dbo].[StockSale] OFF;
GO

-- Copy the data from the DeliveryNoteDetail table to the StockSaleDetail table, maintaining the Id values
SET IDENTITY_INSERT [dbo].[StockSaleDetail] ON;
INSERT INTO [dbo].[StockSaleDetail] ([Id], [StockSaleId], [ProductId], [ProductTypeId], [Quantity], [Deleted], [AmendUserID], [AmendDate])
SELECT [Id], [DeliveryNoteId], [ProductId], [ProductTypeId], [Quantity], [Deleted], [AmendUserID], [AmendDate]
FROM [dbo].[DeliveryNoteDetail]
SET IDENTITY_INSERT [dbo].[StockSaleDetail] OFF;
GO


-- Rename the DeliveryNoteDetailId column in the dbo.Activity table to StockSaleDetailId
EXEC sp_rename 'dbo.Activity.DeliveryNoteDetailId', 'StockSaleDetailId', 'COLUMN';
GO

-- Drop any foreign key constraints in the Activity table that reference the DeliveryNoteDetail table (FK_Activity_DeliveryNoteDetailId)
IF OBJECT_ID('FK_Activity_DeliveryNoteDetailId', 'F') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_DeliveryNoteDetailId];
END
GO

-- Drop the DeliveryNote and DeliveryNoteDetail tables
DROP TABLE [dbo].[DeliveryNoteDetail];
DROP TABLE [dbo].[DeliveryNote];
GO



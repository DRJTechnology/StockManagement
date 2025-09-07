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
    --[DirectSale]  BIT      DEFAULT ((0)) NOT NULL,
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
INSERT INTO [dbo].[StockSale] ([Id], [Date], [LocationId], /* [DirectSale], */ [TransactionId], [Deleted], [AmendUserID], [AmendDate])
SELECT [Id], [Date], [LocationId], /* [DirectSale], */ NULL, [Deleted], [AmendUserID], [AmendDate]
FROM [dbo].[DeliveryNote]
Where [DirectSale] = 1
SET IDENTITY_INSERT [dbo].[StockSale] OFF;
GO

-- Copy the data from the DeliveryNoteDetail table to the StockSaleDetail table, maintaining the Id values
SET IDENTITY_INSERT [dbo].[StockSaleDetail] ON;
INSERT INTO [dbo].[StockSaleDetail] ([Id], [StockSaleId], [ProductId], [ProductTypeId], [Quantity], [Deleted], [AmendUserID], [AmendDate])
SELECT dnd.[Id], dnd.[DeliveryNoteId], dnd.[ProductId], dnd.[ProductTypeId], dnd.[Quantity], dnd.[Deleted], dnd.[AmendUserID], dnd.[AmendDate]
FROM [dbo].[DeliveryNoteDetail] dnd 
INNER JOIN [dbo].[DeliveryNote] dn ON dnd.DeliveryNoteId = dn.Id
Where dn.[DirectSale] = 1
SET IDENTITY_INSERT [dbo].[StockSaleDetail] OFF;
GO


---- Rename the DeliveryNoteDetailId column in the dbo.Activity table to StockSaleDetailId
--EXEC sp_rename 'dbo.Activity.DeliveryNoteDetailId', 'StockSaleDetailId', 'COLUMN';
--GO

---- Drop any foreign key constraints in the Activity table that reference the DeliveryNoteDetail table (FK_Activity_DeliveryNoteDetailId)
--IF OBJECT_ID('FK_Activity_DeliveryNoteDetailId', 'F') IS NOT NULL
--BEGIN
--    ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_DeliveryNoteDetailId];
--END
--GO

-- Drop the DeliveryNote and DeliveryNoteDetail tables
UPDATE  [dbo].[DeliveryNoteDetail]
SET     Deleted  = 1
WHERE   DeliveryNoteId IN (SELECT Id FROM [dbo].[DeliveryNote] WHERE DirectSale = 1);
UPDATE  [dbo].[DeliveryNote] 
SET     Deleted  = 1
WHERE   DirectSale = 1;
GO



-- Recreate Activity table with new column
PRINT N'Dropping Default Constraint [dbo].[DF_Activity_Deleted]...';


GO
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [DF_Activity_Deleted];


GO
PRINT N'Dropping Default Constraint [dbo].[DF_Activity_AmendDate]...';


GO
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [DF_Activity_AmendDate];


GO
PRINT N'Dropping Foreign Key [dbo].[FK_Activity_Location]...';


GO
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_Location];


GO
PRINT N'Dropping Foreign Key [dbo].[FK_Activity_Action]...';


GO
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_Action];


GO
PRINT N'Dropping Foreign Key [dbo].[FK_Activity_Product]...';


GO
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_Product];


GO
PRINT N'Dropping Foreign Key [dbo].[FK_Activity_ProductType]...';


GO
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_ProductType];


GO
PRINT N'Dropping Foreign Key [dbo].[FK_Activity_DeliveryNoteDetailId]...';


GO
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_DeliveryNoteDetailId];


GO

ALTER TABLE [dbo].[StockOrderDetail] DROP CONSTRAINT [FK_StockOrderDetail_ProductType];


GO
PRINT N'Starting rebuilding table [dbo].[Activity]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Activity] (
    [Id]                   INT             IDENTITY (1, 1) NOT NULL,
    [ActivityDate]         DATE            NOT NULL,
    [ActionId]             INT             NOT NULL,
    [ProductId]            INT             NOT NULL,
    [ProductTypeId]        INT             NOT NULL,
    [LocationId]           INT             NOT NULL,
    [Quantity]             INT             NOT NULL,
    [Notes]                NVARCHAR (1024) NULL,
    [DeliveryNoteDetailId] INT             NULL,
    [StockSaleDetailId]    INT             NULL,
    [StockOrderDetailId]   INT             NULL,
    [Deleted]              BIT             CONSTRAINT [DF_Activity_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]          INT             NOT NULL,
    [AmendDate]            DATETIME        CONSTRAINT [DF_Activity_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Activity1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Activity])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Activity] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Activity] ([Id], [ActivityDate], [ActionId], [ProductId], [ProductTypeId], [LocationId], [Quantity], [DeliveryNoteDetailId], [StockOrderDetailId], [Deleted], [AmendUserID], [AmendDate])
        SELECT   [Id],
                 [ActivityDate],
                 [ActionId],
                 [ProductId],
                 [ProductTypeId],
                 [LocationId],
                 [Quantity],
                 [DeliveryNoteDetailId],
                 [StockOrderDetailId],
                 [Deleted],
                 [AmendUserID],
                 [AmendDate]
        FROM     [dbo].[Activity]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Activity] OFF;
    END

DROP TABLE [dbo].[Activity];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Activity]', N'Activity';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Activity1]', N'PK_Activity', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

GO
PRINT N'Creating Foreign Key [dbo].[FK_Activity_Location]...';


GO
ALTER TABLE [dbo].[Activity] WITH NOCHECK
    ADD CONSTRAINT [FK_Activity_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Activity_Action]...';


GO
ALTER TABLE [dbo].[Activity] WITH NOCHECK
    ADD CONSTRAINT [FK_Activity_Action] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Action] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Activity_Product]...';


GO
ALTER TABLE [dbo].[Activity] WITH NOCHECK
    ADD CONSTRAINT [FK_Activity_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Activity_ProductType]...';


GO
ALTER TABLE [dbo].[Activity] WITH NOCHECK
    ADD CONSTRAINT [FK_Activity_ProductType] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductType] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Activity_DeliveryNoteDetailId]...';


GO
ALTER TABLE [dbo].[Activity] WITH NOCHECK
    ADD CONSTRAINT [FK_Activity_DeliveryNoteDetailId] FOREIGN KEY ([DeliveryNoteDetailId]) REFERENCES [dbo].[DeliveryNoteDetail] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Activity_StockSaleDetailId]...';


GO
ALTER TABLE [dbo].[Activity] WITH NOCHECK
    ADD CONSTRAINT [FK_Activity_StockSaleDetailId] FOREIGN KEY ([StockSaleDetailId]) REFERENCES [dbo].[StockSaleDetail] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Activity_StockOrderDetailId]...';


GO
ALTER TABLE [dbo].[Activity] WITH NOCHECK
    ADD CONSTRAINT [FK_Activity_StockOrderDetailId] FOREIGN KEY ([StockOrderDetailId]) REFERENCES [dbo].[StockOrderDetail] ([Id]);

GO

GO
PRINT N'Checking existing data against newly created constraints';

GO
ALTER TABLE [dbo].[Activity] WITH CHECK CHECK CONSTRAINT [FK_Activity_Location];

ALTER TABLE [dbo].[Activity] WITH CHECK CHECK CONSTRAINT [FK_Activity_Action];

ALTER TABLE [dbo].[Activity] WITH CHECK CHECK CONSTRAINT [FK_Activity_Product];

ALTER TABLE [dbo].[Activity] WITH CHECK CHECK CONSTRAINT [FK_Activity_ProductType];

ALTER TABLE [dbo].[Activity] WITH CHECK CHECK CONSTRAINT [FK_Activity_DeliveryNoteDetailId];

ALTER TABLE [dbo].[Activity] WITH CHECK CHECK CONSTRAINT [FK_Activity_StockSaleDetailId];

ALTER TABLE [dbo].[Activity] WITH CHECK CHECK CONSTRAINT [FK_Activity_StockOrderDetailId];

GO

-- Update the Activity records to copy old DeliveryNoteDetailId to StockOrderDetailId for direct sales
UPDATE a
SET StockSaleDetailId = DeliveryNoteDetailId,
	DeliveryNoteDetailId = NULL
FROM	dbo.Activity a
INNER JOIN dbo.DeliveryNoteDetail dnd ON a.DeliveryNoteDetailId = dnd.Id
INNER JOIN dbo.DeliveryNote dn ON dnd.DeliveryNoteId = dn.Id
WHERE dn.DirectSale = 1;
GO

-- Drop the DirectSale column from the DeliveryNote table
ALTER TABLE [dbo].[DeliveryNote] DROP CONSTRAINT [DF__DeliveryN__Direc__30C33EC3];
GO
ALTER TABLE [dbo].[DeliveryNote] DROP COLUMN [DirectSale];
GO

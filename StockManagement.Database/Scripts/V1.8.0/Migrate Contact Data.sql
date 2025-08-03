---------------------
-- PRE-RELEASE SCRIPT
---------------------
EXEC sp_rename 'dbo.Activity.VenueId', 'LocationId', 'COLUMN';

ALTER TABLE dbo.Activity
DROP CONSTRAINT FK_Activity_Venue;
GO

EXEC sp_rename 'dbo.DeliveryNote.VenueId', 'LocationId', 'COLUMN';

ALTER TABLE dbo.DeliveryNote
DROP CONSTRAINT FK_DeliveryNote_Venue;
GO

CREATE TABLE [dbo].[Location] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50)   NOT NULL,
    [Notes]        NVARCHAR (4000) NULL,
    [Deleted]      BIT             CONSTRAINT [DF_Location_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]  INT             NOT NULL,
    [AmendDate]    DATETIME        CONSTRAINT [DF_Location_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[ContactType] (
    [Id]          SMALLINT      NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    [Deleted]     BIT           DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT           NOT NULL,
    [AmendDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED ([Id] ASC),
);
GO

CREATE TABLE [dbo].[Contact] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100)  NOT NULL,
    [ContactTypeId] SMALLINT        NOT NULL,
    [Notes]         NVARCHAR (4000) NULL,
    [Deleted]       BIT             DEFAULT ((0)) NOT NULL,
    [AmendUserID]   INT             NOT NULL,
    [AmendDate]     DATETIME        NOT NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Contact_ContactType] FOREIGN KEY ([ContactTypeId]) REFERENCES [ContactType]([Id])
);
GO

-- Populate Contact Types
INSERT INTO [dbo].[ContactType] (Id,[Name],Deleted,AmendUserId,AmendDate)
VALUES (1, 'Supplier',0,0,GETDATE())
INSERT INTO [dbo].[ContactType] (Id,[Name],Deleted,AmendUserId,AmendDate)
VALUES (2, 'Customer',0,0,GETDATE())
INSERT INTO [dbo].[ContactType] (Id,[Name],Deleted,AmendUserId,AmendDate)
VALUES (3, 'Customer and Supplier',0,0,GETDATE())
GO

-- Migrate suppliers
SET IDENTITY_INSERT dbo.[Contact] ON
INSERT INTO dbo.Contact (Id, [Name], ContactTypeId, Notes, Deleted, AmendUserID, AmendDate)
SELECT  Id, SupplierName, 1, NULL, Deleted, AmendUserID, AmendDate
FROM	dbo.Supplier
SET IDENTITY_INSERT dbo.[Contact] OFF
GO

-- Migrate Venues to Locations
SET IDENTITY_INSERT dbo.[Location] ON
INSERT INTO dbo.[Location] (Id, [Name], Notes, Deleted, AmendUserID, AmendDate)
SELECT	Id, [VenueName], Notes, Deleted, AmendUserID, AmendDate
FROM	dbo.Venue
SET IDENTITY_INSERT dbo.[Location] OFF
GO


ALTER TABLE dbo.Activity
ADD CONSTRAINT FK_Activity_Location
FOREIGN KEY (LocationId) REFERENCES dbo.Location(Id);
GO

ALTER TABLE dbo.DeliveryNote
ADD CONSTRAINT FK_DeliveryNote_Location
FOREIGN KEY (LocationId) REFERENCES dbo.Location(Id);
GO

EXEC sp_rename 'dbo.StockReceipt.SupplierId', 'ContactId', 'COLUMN';

ALTER TABLE dbo.StockReceipt
DROP CONSTRAINT FK_StockReceipt_Supplier;
GO

ALTER TABLE dbo.StockReceipt
ADD CONSTRAINT FK_StockReceipt_Contact
FOREIGN KEY (ContactId) REFERENCES dbo.Contact(Id);
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier]') AND type in (N'U'))
DROP TABLE [dbo].[Supplier]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Venue]') AND type in (N'U'))
DROP TABLE [dbo].[Venue]
GO



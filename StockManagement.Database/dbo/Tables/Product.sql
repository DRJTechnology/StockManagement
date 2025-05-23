CREATE TABLE [dbo].[Product] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ProductName] NVARCHAR (50) NOT NULL,
    [Deleted]     BIT           CONSTRAINT [DF_Product_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT           NOT NULL,
    [AmendDate]   DATETIME      CONSTRAINT [DF_Product_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC)
);


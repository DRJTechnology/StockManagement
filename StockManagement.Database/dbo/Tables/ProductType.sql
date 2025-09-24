CREATE TABLE [dbo].[ProductType] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [ProductTypeName]  NVARCHAR (50) NOT NULL,
    [DefaultCostPrice] MONEY         NULL,
    [DefaultSalePrice] MONEY         NULL,
    [Deleted]          BIT           CONSTRAINT [DF_ProductType_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]      INT           NOT NULL,
    [AmendDate]        DATETIME      CONSTRAINT [DF_ProductType_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


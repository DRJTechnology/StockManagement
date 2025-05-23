CREATE TABLE [dbo].[ProductProductType] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [ProductId]     INT      NOT NULL,
    [ProductTypeId] INT      NOT NULL,
    [Deleted]       BIT      CONSTRAINT [DF_ProductProductType_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]   INT      NOT NULL,
    [AmendDate]     DATETIME CONSTRAINT [DF_ProductProductType_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_ProductProductType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductProductType_ProductProductType] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);


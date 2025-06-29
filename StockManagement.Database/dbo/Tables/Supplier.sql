CREATE TABLE [dbo].[Supplier] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [SupplierName] NVARCHAR (50) NOT NULL,
    [Deleted]      BIT           CONSTRAINT [DF_Supplier_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]  INT           NOT NULL,
    [AmendDate]    DATETIME      CONSTRAINT [DF_Supplier_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([Id] ASC)
);


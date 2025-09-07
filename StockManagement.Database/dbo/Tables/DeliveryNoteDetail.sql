CREATE TABLE [dbo].[DeliveryNoteDetail] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [DeliveryNoteId] INT      NOT NULL,
    [ProductId]      INT      NOT NULL,
    [ProductTypeId]  INT      NOT NULL,
    [Quantity]       INT      NOT NULL,
    [Deleted]        BIT      CONSTRAINT [DF_DeliveryNoteDetail_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]    INT      NOT NULL,
    [AmendDate]      DATETIME CONSTRAINT [DF_DeliveryNoteDetail_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_DeliveryNoteDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DeliveryNoteDetail_DeliveryNote] FOREIGN KEY ([DeliveryNoteId]) REFERENCES [dbo].[DeliveryNote] ([Id]),
    CONSTRAINT [FK_DeliveryNoteDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_DeliveryNoteDetail_ProductType] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductType] ([Id])
);


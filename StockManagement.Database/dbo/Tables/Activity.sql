﻿CREATE TABLE [dbo].[Activity] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [ActivityDate]          DATE     NOT NULL,
    [ActionId]              INT      NOT NULL,
    [ProductId]             INT      NOT NULL,
    [ProductTypeId]         INT      NOT NULL,
    [VenueId]               INT      NOT NULL,
    [Quantity]              INT      NOT NULL,
    [DeliveryNoteDetailId]  INT      NULL,
    [StockReceiptDetailId]  INT      NULL,
    [Deleted]               BIT      CONSTRAINT [DF_Activity_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]           INT      NOT NULL,
    [AmendDate]             DATETIME CONSTRAINT [DF_Activity_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Activity_Action] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Action] ([Id]),
    CONSTRAINT [FK_Activity_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_Activity_ProductType] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductType] ([Id]),
    CONSTRAINT [FK_Activity_Venue] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venue] ([Id]),
    CONSTRAINT [FK_Activity_DeliveryNoteDetailId] FOREIGN KEY ([DeliveryNoteDetailId]) REFERENCES [dbo].[DeliveryNoteDetail] ([Id])
);


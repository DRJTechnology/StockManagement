CREATE TABLE [dbo].[DeliveryNote] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME NOT NULL,
    [VenueId]     INT      NOT NULL,
    [DirectSale]  BIT      DEFAULT ((0)) NOT NULL,
    [Deleted]     BIT      CONSTRAINT [DF_DeliveryNote_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT      NOT NULL,
    [AmendDate]   DATETIME CONSTRAINT [DF_DeliveryNote_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_DeliveryNote] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DeliveryNote_Venue] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venue] ([Id])
);


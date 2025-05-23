CREATE TABLE [dbo].[Venue] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [VenueName]   NVARCHAR (50) NOT NULL,
    [Deleted]     BIT           CONSTRAINT [DF_Venue_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT           NOT NULL,
    [AmendDate]   DATETIME      CONSTRAINT [DF_Venue_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Venue] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[ContactType] (
    [Id]          SMALLINT      NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    [Deleted]     BIT           DEFAULT ((0)) NOT NULL,
    [AmendUserID] INT           NOT NULL,
    [AmendDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED ([Id] ASC),
);


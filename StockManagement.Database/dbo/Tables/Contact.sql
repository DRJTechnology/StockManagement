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


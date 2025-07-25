CREATE TABLE [dbo].[Location] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50)   NOT NULL,
    [Notes]        NVARCHAR (4000) NULL,
    [Deleted]      BIT             CONSTRAINT [DF_Location_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]  INT             NOT NULL,
    [AmendDate]    DATETIME        CONSTRAINT [DF_Location_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC)
);


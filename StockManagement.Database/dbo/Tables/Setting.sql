CREATE TABLE [dbo].[Setting] (
    [Id]              INT            NOT NULL,
    [SettingName]     NVARCHAR (50)  NOT NULL,
    [SettingValue]    NVARCHAR (255) NOT NULL,
    [Deleted]         BIT            CONSTRAINT [DF_Setting_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]     INT            NOT NULL,
    [AmendDate]       DATETIME       CONSTRAINT [DF_Setting_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([Id] ASC)
);


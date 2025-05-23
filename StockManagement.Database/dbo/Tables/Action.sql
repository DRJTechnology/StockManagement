CREATE TABLE [dbo].[Action] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [ActionName]      NVARCHAR (50) NOT NULL,
    [Direction]       SMALLINT      NOT NULL,
    [AffectStockRoom] BIT           CONSTRAINT [DF_Action_AffectStockRoom] DEFAULT ((0)) NOT NULL,
    [Deleted]         BIT           CONSTRAINT [DF_Action_Deleted] DEFAULT ((0)) NOT NULL,
    [AmendUserID]     INT           NOT NULL,
    [AmendDate]       DATETIME      CONSTRAINT [DF_Action_AmendDate] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED ([Id] ASC)
);


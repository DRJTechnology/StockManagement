CREATE TABLE [finance].[Invoice] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [InvoiceNumber] NVARCHAR (50)   NOT NULL,
    [InvoiceDate]   DATE            NOT NULL,
    [DueDate]       DATE            NOT NULL,
    [Description]   NVARCHAR (512)  NOT NULL,
    [Amount]        DECIMAL (18, 2) NOT NULL,
    [Deleted]       BIT             DEFAULT ((0)) NOT NULL,
    [CreateUserId]  INT             NOT NULL,
    [CreateDate]    DATETIME        CONSTRAINT [DF_Invoice_CreateDate] DEFAULT (getutcdate()) NOT NULL,
    [AmendUserId]   INT             NOT NULL,
    [AmendDate]     DATETIME        CONSTRAINT [DF_Invoice_AmendDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);


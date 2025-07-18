CREATE TABLE [finance].[InvoiceDetail] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [InvoiceId]     INT      NOT NULL,
    [TransactionId] INT      NULL,
    [Deleted]       BIT      DEFAULT ((0)) NOT NULL,
    [CreateUserId]  INT      NOT NULL,
    [CreateDate]    DATETIME CONSTRAINT [DF_InvoiceDetail_CreateDate] DEFAULT (getutcdate()) NOT NULL,
    [AmendUserId]   INT      NOT NULL,
    [AmendDate]     DATETIME CONSTRAINT [DF_InvoiceDetail_AmendDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_InvoiceDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvoiceDetail_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [finance].[Invoice] ([Id]),
    CONSTRAINT [FK_InvoiceDetail_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [finance].[Transaction] ([Id])
);


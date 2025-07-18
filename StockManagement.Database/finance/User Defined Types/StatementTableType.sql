CREATE TYPE [finance].[StatementTableType] AS TABLE (
    [Date]            DATETIME       NULL,
    [Amount]          MONEY          NULL,
    [Description]     NVARCHAR (512) NULL,
    [TransactionType] NVARCHAR (512) NULL);


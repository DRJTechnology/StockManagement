CREATE TYPE [finance].[StockSaleConfirmTableType] AS TABLE (
    [UnitPrice]             MONEY   NULL,
    [StockSaleDetailId]     INT     NULL,
    [ProductId]             INT     NULL,
    [ProductTypeId]         INT     NULL,
    [Quantity]              INT     NULL
);


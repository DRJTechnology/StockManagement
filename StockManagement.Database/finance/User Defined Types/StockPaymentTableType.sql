CREATE TYPE [finance].[StockPaymentTableType] AS TABLE (
    [UnitPrice]             MONEY   NULL,
    [StockOrderDetailId]    INT     NULL,
    [ProductId]             INT     NULL,
    [ProductTypeId]         INT     NULL,
    [Quantity]              INT     NULL
);


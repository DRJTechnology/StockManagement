
-- Rename the TransactionId column in the dbo.StockSaleDetail table to TransactionDetailId
EXEC sp_rename 'dbo.StockSaleDetail.TransactionId', 'TransactionDetailId', 'COLUMN';
GO

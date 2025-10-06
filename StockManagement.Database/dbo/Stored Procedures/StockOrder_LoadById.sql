-- ===========================================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Get Stock Order
-- ===========================================================================
CREATE PROCEDURE [dbo].[StockOrder_LoadById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		so.[Id],
		so.[Date],
		so.[ContactId],
		c.[Name] AS ContactName,
		so.TotalCost,
		so.[PaymentRecorded],
		so.[StockReceiptRecorded],
		so.[Deleted],
		so.[AmendUserID],
		so.[AmendDate]
	FROM [StockOrder] so
	INNER JOIN [Contact] c ON so.ContactId = c.Id
	WHERE
		so.[Deleted] <> 1
		AND so.[Id] = @Id

	SELECT
		sod.[Id],
		sod.[ProductId],
		p.[ProductName],
		sod.[ProductTypeId],
		pt.[ProductTypeName],
		sod.Quantity,
		sod.[Deleted],
		sod.[AmendUserID],
		sod.[AmendDate]
	FROM [StockOrderDetail] sod
	INNER JOIN [Product] p ON sod.ProductId = p.Id
	INNER JOIN [ProductType] pt ON sod.ProductTypeId = pt.Id
	WHERE
		sod.[Deleted] <> 1
		AND sod.[StockOrderId] = @Id

	SET @Err = @@Error

	RETURN @Err
END
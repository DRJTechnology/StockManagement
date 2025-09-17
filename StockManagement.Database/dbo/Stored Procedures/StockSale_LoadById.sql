-- ===========================================================================
-- Author:		Dave Brown
-- Create date: 29 Jun 2025
-- Description:	Get Delivery Note
-- ===========================================================================
-- 02 JUL 2025 - Dave Brown - include DirectSale column
-- 03 JUL 2025 - Dave Brown - include stock sale detail child records
-- ===========================================================================
CREATE PROCEDURE [dbo].[StockSale_LoadById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		ss.[Id],
		ss.[Date],
		ss.[LocationId],
		l.[Name] AS LocationName,
		ss.[ContactId],
		c.[Name] AS ContactName,
		ss.TotalPrice,
		ss.SaleConfirmed,
		ss.PaymentReceived,
		ss.[Deleted],
		ss.[AmendUserID],
		ss.[AmendDate]
	FROM [StockSale] ss
	INNER JOIN [Location] l ON ss.LocationId = l.Id
	INNER JOIN [Contact] c ON ss.ContactId = c.Id
	WHERE
		ss.[Deleted] <> 1
		AND ss.[Id] = @Id

	SELECT
		ssd.[Id],
		ssd.[StockSaleId],
		ssd.[ProductId],
		p.[ProductName],
		ssd.[ProductTypeId],
		pt.[ProductTypeName],
		ssd.Quantity,
		ssd.UnitPrice,
		ssd.[Deleted],
		ssd.[AmendUserID],
		ssd.[AmendDate]
	FROM [StockSaleDetail] ssd
	INNER JOIN [Product] p ON ssd.ProductId = p.Id
	INNER JOIN [ProductType] pt ON ssd.ProductTypeId = pt.Id
	WHERE
		ssd.[Deleted] <> 1
		AND ssd.[StockSaleId] = @Id

	SET @Err = @@Error

	RETURN @Err
END
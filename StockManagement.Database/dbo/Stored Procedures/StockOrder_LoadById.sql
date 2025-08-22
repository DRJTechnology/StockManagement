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
		sr.[Id],
		sr.[Date],
		sr.[ContactId],
		c.[Name] AS ContactName,
		sr.[Deleted],
		sr.[AmendUserID],
		sr.[AmendDate]
	FROM [StockOrder] sr
	INNER JOIN [Contact] c ON sr.ContactId = c.Id
	WHERE
		sr.[Deleted] <> 1
		AND sr.[Id] = @Id

	SELECT
		srd.[Id],
		srd.[ProductId],
		p.[ProductName],
		srd.[ProductTypeId],
		pt.[ProductTypeName],
		srd.Quantity,
		srd.[Deleted],
		srd.[AmendUserID],
		srd.[AmendDate]
	FROM [StockOrderDetail] srd
	INNER JOIN [Product] p ON srd.ProductId = p.Id
	INNER JOIN [ProductType] pt ON srd.ProductTypeId = pt.Id
	WHERE
		srd.[Deleted] <> 1
		AND srd.[StockOrderId] = @Id

	SET @Err = @@Error

	RETURN @Err
END
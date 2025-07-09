-- ===========================================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Get Stock Receipt
-- ===========================================================================
CREATE PROCEDURE [dbo].[StockReceipt_LoadById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		sr.[Id],
		sr.[Date],
		sr.[SupplierId],
		s.[SupplierName],
		sr.[Deleted],
		sr.[AmendUserID],
		sr.[AmendDate]
	FROM [StockReceipt] sr
	INNER JOIN [Supplier] s ON sr.SupplierId = s.Id
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
	FROM [StockReceiptDetail] srd
	INNER JOIN [Product] p ON srd.ProductId = p.Id
	INNER JOIN [ProductType] pt ON srd.ProductTypeId = pt.Id
	WHERE
		srd.[Deleted] <> 1
		AND srd.[StockReceiptId] = @Id

	SET @Err = @@Error

	RETURN @Err
END
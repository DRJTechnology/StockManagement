-- =========================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Get Stock Receipts
-- =========================================================
CREATE PROCEDURE [dbo].[StockReceipt_LoadAll]
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
	ORDER BY
		sr.[Date] DESC

	SET @Err = @@Error

	RETURN @Err
END
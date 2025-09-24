-- =========================================================
-- Author:		Dave Brown
-- Create date: 29 Jun 2025
-- Description:	Get Delivery Notes
-- =========================================================
-- 02 JUL 2025 - Dave Brown - include DirectSale column
-- =========================================================
CREATE PROCEDURE [dbo].[StockSale_LoadAll]
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
	ORDER BY
		[Date] DESC

	SET @Err = @@Error

	RETURN @Err
END
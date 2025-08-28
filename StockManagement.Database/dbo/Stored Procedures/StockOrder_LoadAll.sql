-- =========================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Get Stock Orders
-- =========================================================
CREATE PROCEDURE [dbo].[StockOrder_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		so.[Id],
		so.[Date],
		so.[ContactId],
		c.[Name] AS ContactName,
		so.[PaymentRecorded],
		so.[StockReceiptRecorded],
		so.[Deleted],
		so.[AmendUserID],
		so.[AmendDate]
	FROM [StockOrder] so
	INNER JOIN [Contact] c ON so.ContactId = c.Id
	WHERE
		so.[Deleted] <> 1
	ORDER BY
		so.[Date] DESC

	SET @Err = @@Error

	RETURN @Err
END
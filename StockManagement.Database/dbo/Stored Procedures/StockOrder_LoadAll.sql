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
	ORDER BY
		sr.[Date] DESC

	SET @Err = @@Error

	RETURN @Err
END
﻿-- =========================================================
-- Author:		Dave Brown
-- Create date: 29 Jun 2025
-- Description:	Get Delivery Notes
-- =========================================================
-- 02 JUL 2025 - Dave Brown - include DirectSale column
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNote_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		dn.[Id],
		dn.[Date],
		dn.[VenueId],
		v.[VenueName],
		dn.DirectSale,
		dn.[Deleted],
		dn.[AmendUserID],
		dn.[AmendDate]
	FROM [DeliveryNote] dn
	INNER JOIN [Venue] v ON dn.VenueId = v.Id
	WHERE
		dn.[Deleted] <> 1
	ORDER BY
		[Date] DESC

	SET @Err = @@Error

	RETURN @Err
END
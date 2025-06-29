-- =========================================================
-- Author:		Dave Brown
-- Create date: 29 Jun 2025
-- Description:	Get Delivery Note
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNote_LoadById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		dn.[Id],
		dn.[Date],
		dn.[VenueId],
		v.[VenueName],
		dn.[Deleted],
		dn.[AmendUserID],
		dn.[AmendDate]
	FROM [DeliveryNote] dn
	INNER JOIN [Venue] v ON dn.VenueId = v.Id
	WHERE
		dn.[Deleted] <> 1
		AND dn.[Id] = @Id

	SET @Err = @@Error

	RETURN @Err
END
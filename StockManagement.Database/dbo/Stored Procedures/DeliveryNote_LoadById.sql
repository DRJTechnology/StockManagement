-- ===========================================================================
-- Author:		Dave Brown
-- Create date: 29 Jun 2025
-- Description:	Get Delivery Note
-- ===========================================================================
-- 02 JUL 2025 - Dave Brown - include DirectSale column
-- 03 JUL 2025 - Dave Brown - include delivery note detail child records
-- ===========================================================================
CREATE PROCEDURE [dbo].[DeliveryNote_LoadById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		dn.[Id],
		dn.[Date],
		dn.[LocationId],
		l.[Name] AS LocationName,
		dn.DirectSale,
		dn.[Deleted],
		dn.[AmendUserID],
		dn.[AmendDate]
	FROM [DeliveryNote] dn
	INNER JOIN [Location] l ON dn.LocationId = l.Id
	WHERE
		dn.[Deleted] <> 1
		AND dn.[Id] = @Id

	SELECT
		dnd.[Id],
		dnd.[ProductId],
		p.[ProductName],
		dnd.[ProductTypeId],
		pt.[ProductTypeName],
		dnd.Quantity,
		dnd.[Deleted],
		dnd.[AmendUserID],
		dnd.[AmendDate]
	FROM [DeliveryNoteDetail] dnd
	INNER JOIN [Product] p ON dnd.ProductId = p.Id
	INNER JOIN [ProductType] pt ON dnd.ProductTypeId = pt.Id
	WHERE
		dnd.[Deleted] <> 1
		AND dnd.[DeliveryNoteId] = @Id

	SET @Err = @@Error

	RETURN @Err
END
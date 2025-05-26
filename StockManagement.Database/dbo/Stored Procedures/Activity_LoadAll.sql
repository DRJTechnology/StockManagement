
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Activity
-- =========================================================
CREATE PROCEDURE [dbo].[Activity_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		a.[Id],
		a.[ActivityDate],
		a.[ActionId],
		act.[ActionName],
		a.[ProductId],
		p.[ProductName],
		a.[ProductTypeId],
		pt.[ProductTypeName],
		a.[VenueId],
		v.[VenueName],
		a.[Quantity],
		a.[Deleted],
		a.[AmendUserID],
		a.[AmendDate]
	FROM [Activity] a
	INNER JOIN [Product] p ON a.[ProductId] = p.Id
	INNER JOIN [ProductType] pt ON a.[ProductTypeId] = pt.Id
	INNER JOIN [Venue] v ON a.[VenueId] = v.Id
	INNER JOIN [Action] act ON a.[ActionId] = act.Id
	WHERE
		a.[Deleted] <> 1
	ORDER BY
		a.[ActivityDate] DESC,
		p.[ProductName] ASC,
		pt.[ProductTypeName] ASC,
		v.[VenueName] ASC

	SET @Err = @@Error

	RETURN @Err
END
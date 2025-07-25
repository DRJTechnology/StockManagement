
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Get Contacts
-- =========================================================
-- 11 Jun 2025 - DB Updated to include Notes field
-- 24 Jun 2025 - DB Locations and Suppliers replaced by Contacts
-- =========================================================
CREATE PROCEDURE [dbo].[Contact_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		c.[Id],
		c.[ContactTypeId],
		ct.[Name] AS 'Type',
		c.[Name],
		c.[Notes],
		c.[Deleted],
		c.[AmendUserID],
		c.[AmendDate]
	FROM [Contact] c
	INNER JOIN dbo.[ContactType] ct ON c.ContactTypeId = ct.Id
	WHERE
		c.[Deleted] <> 1
	ORDER BY
		c.[Name] ASC

	SET @Err = @@Error

	RETURN @Err
END
-- =========================================================
-- Author:		Dave Brown
-- Create date: 24 May 2025
-- Description:	Get Contacts by Contact Type
-- =========================================================
CREATE PROCEDURE [dbo].[Contact_LoadByType]
(
	@ContactTypeId int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		c.[Id],
		c.[ContactTypeId],
		c.[Name],
		ct.[Name] AS 'Type',
		c.[Notes],
		c.[Deleted],
		c.[AmendUserID],
		c.[AmendDate]
	FROM [Contact] c
	INNER JOIN dbo.[ContactType] ct ON c.ContactTypeId = ct.Id
	WHERE	c.[Deleted] <> 1
		AND (c.ContactTypeId = 3 OR c.ContactTypeId = @ContactTypeId)
	ORDER BY
		c.[Name] ASC

	SET @Err = @@Error

	RETURN @Err
END
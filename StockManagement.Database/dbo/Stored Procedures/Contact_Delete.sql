-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete Contact
-- =========================================================
-- 24 Jun 2025 - DB Locations and Suppliers replaced by Contacts
-- =========================================================
CREATE PROCEDURE [dbo].[Contact_Delete]
(
	@Success bit output,
	@ContactID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Contact]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @ContactID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
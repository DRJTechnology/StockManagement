-- ============================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update Contact
-- ============================================================
-- 11 Jun 2025 - DB Updated to include Notes field
-- 24 Jun 2025 - DB Locations and Suppliers replaced by Contacts
-- ============================================================
CREATE PROCEDURE [dbo].[Contact_Update]
(
	@Success bit output,
	@Id int,
	@ContactTypeId int,
	@Name nvarchar(50),
	@Notes nvarchar(4000),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Contact]
	SET
		[ContactTypeId] = @ContactTypeId,
		[Name] = @Name,
		[Notes] = @Notes,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
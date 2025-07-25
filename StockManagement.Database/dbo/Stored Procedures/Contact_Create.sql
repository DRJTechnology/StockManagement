-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Contact
-- =========================================================
-- 11 Jun 2025 - DB Updated to include Notes field
-- 24 Jun 2025 - DB Locations and Suppliers replaced by Contacts
-- =========================================================
CREATE PROCEDURE [dbo].[Contact_Create]
(
	@Success bit output,
	@Id int output,
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

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Contact] ([ContactTypeId],[Name],[Notes],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@ContactTypeId, @Name, @Notes, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
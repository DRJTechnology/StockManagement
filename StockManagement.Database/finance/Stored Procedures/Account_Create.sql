-- ==========================================================
-- Author:		Dave Brown
-- Create date: 21 Jul 2025
-- Description:	Creates an account record
-- ==========================================================
CREATE PROCEDURE [finance].[Account_Create]
	@Success			BIT OUTPUT,
	@Id					INT OUTPUT, 
	@AccountTypeId		INT,
	@Name				NVARCHAR(255),
	@Notes				NVARCHAR(4000) = NULL,
	@Active				BIT,
	@CurrentUserId		INT
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

    INSERT INTO [finance].[Account] (AccountTypeId, [Name], Notes, Active, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
	VALUES (@AccountTypeId, @Name, @Notes, @Active, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME())

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

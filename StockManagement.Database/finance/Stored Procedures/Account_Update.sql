-- =============================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Updates an account record
-- =============================================
CREATE PROCEDURE [finance].[Account_Update]
	@Success bit output,
	@Id					INT, 
	@AccountTypeId		INT,
	@Name				NVARCHAR(50),
	@Notes				NVARCHAR(4000) = NULL,
	@Active				BIT,
	@CurrentUserId		INT
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE	[finance].[Account]
	SET		AccountTypeId = @AccountTypeId,
			[Name] = @Name,
			Notes = @Notes,
			Active = @Active,
			AmendUserId = @CurrentUserId,
			AmendDate = SYSDATETIME()
	WHERE	Id = @Id


	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
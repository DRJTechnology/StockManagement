-- =============================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Updates an account record
-- =============================================
CREATE PROCEDURE [finance].[Account_Update]
	@Id					INT, 
	@AccountTypeId		INT,
	@Name				NVARCHAR(50),
	@Notes				NVARCHAR(4000) = NULL,
	@Active				BIT,
	@CurrentUserId		INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE	[finance].[Account]
	SET		AccountTypeId = @AccountTypeId,
			[Name] = @Name,
			Notes = @Notes,
			Active = @Active,
			AmendUserId = @CurrentUserId,
			AmendDate = SYSDATETIME()
	WHERE	Id = @Id

END
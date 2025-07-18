-- ===========================================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Updates an account record to deleted
-- ===========================================================
CREATE PROCEDURE [finance].[Account_Delete]
	@Id					INT, 
	@CurrentUserId		INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE	[finance].[Account]
	SET		Deleted = 1,
			AmendUserId = @CurrentUserId,
			AmendDate = SYSDATETIME()
	WHERE	Id = @Id

END
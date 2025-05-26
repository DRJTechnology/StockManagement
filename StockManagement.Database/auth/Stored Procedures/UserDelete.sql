-- ====================================================
-- Author:		Dave Brown
-- Create date: 04 Jan 2024
-- Description:	Delete a user record
-- ====================================================
CREATE PROCEDURE [auth].[UserDelete]
(
	@UserId	INT
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	IF EXISTS (SELECT 1 FROM [auth].AspNetUserTokens WHERE UserId = @UserId)
	BEGIN
		DELETE FROM [auth].AspNetUserTokens
		WHERE UserId = @UserId
	END
	
	IF EXISTS (SELECT 1 FROM [auth].AspNetUserClaims WHERE UserId = @UserId)
	BEGIN
		DELETE FROM [auth].AspNetUserClaims
		WHERE UserId = @UserId
	END
	
	IF EXISTS (SELECT 1 FROM [auth].AspNetUserLogins WHERE UserId = @UserId)
	BEGIN
		DELETE FROM [auth].AspNetUserLogins
		WHERE UserId = @UserId
	END

	IF EXISTS (SELECT 1 FROM [auth].AspNetUserRoles WHERE UserId = @UserId)
	BEGIN
		DELETE FROM [auth].AspNetUserRoles
		WHERE UserId = @UserId
	END

	DELETE FROM [auth].[AspNetUsers]
	WHERE	Id = @UserId

	SET @Err = @@Error

	RETURN @Err
END
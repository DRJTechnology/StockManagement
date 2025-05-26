-- ====================================================
-- Author:		Dave Brown
-- Create date: 03 Jan 2024
-- Description:	Get a user login records by user id
-- ====================================================
CREATE PROCEDURE [auth].[UserLoginLoadByUserId]
(
	@UserId	int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[LoginProvider],
		[ProviderKey],
		[ProviderDisplayName],
		[UserId]
	FROM [auth].[AspNetUserLogins]
	WHERE
		([UserId] = @UserId)

	SET @Err = @@Error

	RETURN @Err
END
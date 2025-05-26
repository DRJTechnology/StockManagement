-- =========================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Get a user record by external login provider
-- =========================================================
CREATE PROCEDURE [auth].[UserLoadByLoginProvider]
(
	@LoginProvider	nvarchar(450),
	@ProviderKey	nvarchar(450)
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	DECLARE @count	int

	SELECT
		u.[Id],
		u.[UserName],
		u.[NormalizedUserName],
		u.[Email],
		u.[NormalizedEmail],
		u.[FirstName],
		u.[LastName],
		u.[EmailConfirmed],
		u.[PasswordHash],
		u.[SecurityStamp],
		u.[ConcurrencyStamp],
		u.[PhoneNumber],
		u.[PhoneNumberConfirmed],
		u.[TwoFactorEnabled],
		u.[LockoutEnd],
		u.[LockoutEnabled],
		u.[AccessFailedCount]
	FROM [auth].[AspNetUsers] u
	INNER JOIN [auth].[AspNetUserLogins] ul ON u.Id = ul.UserId
	WHERE ul.LoginProvider = @LoginProvider
		AND ul.ProviderKey = @ProviderKey

	SET @Err = @@Error

	RETURN @Err
END
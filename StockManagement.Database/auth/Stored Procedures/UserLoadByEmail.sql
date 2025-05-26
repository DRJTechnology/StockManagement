-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Get a record by email
-- ====================================================
CREATE PROCEDURE [auth].[UserLoadByEmail]
(
	@NormalizedEmailAddress nvarchar(256)
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	DECLARE @count	int

	SELECT
		[Id],
		[UserName],
		[NormalizedUserName],
		[Email],
		[NormalizedEmail],
		[FirstName],
		[LastName],
		[EmailConfirmed],
		[PasswordHash],
		[SecurityStamp],
		[ConcurrencyStamp],
		[PhoneNumber],
		[PhoneNumberConfirmed],
		[TwoFactorEnabled],
		[LockoutEnd],
		[LockoutEnabled],
		[AccessFailedCount]
	FROM [auth].[AspNetUsers]
	WHERE
		([NormalizedEmail] = @NormalizedEmailAddress)

	SET @Err = @@Error

	RETURN @Err
END
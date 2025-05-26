-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Get a record by id
-- ====================================================
CREATE PROCEDURE [auth].[UserLoadById]
(
	@Id	int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

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
		([Id] = @Id)

	SET @Err = @@Error

	RETURN @Err
END
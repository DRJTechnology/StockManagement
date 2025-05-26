-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Update a user record
-- ====================================================
CREATE PROCEDURE [auth].[UserUpdate]
(
	@Id						INT,
	@UserName				NVARCHAR(256),
	@NormalizedUserName		NVARCHAR(256),
	@Email					NVARCHAR(256),
	@NormalizedEmail		NVARCHAR(256),
	@FirstName				NVARCHAR(128),
	@LastName				NVARCHAR(128),
	@EmailConfirmed			BIT,
	@PasswordHash			NVARCHAR(MAX),
	@SecurityStamp			NVARCHAR(MAX),
	@ConcurrencyStamp		NVARCHAR(MAX),
	@PhoneNumber			NVARCHAR(MAX),
	@PhoneNumberConfirmed	BIT,
	@TwoFactorEnabled		BIT,
	@LockoutEnd				DATETIMEOFFSET(7),
	@LockoutEnabled			BIT,
	@AccessFailedCount		INT
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [auth].[AspNetUsers]
	SET
		[UserName] = @UserName,
		[NormalizedUserName] = @NormalizedUserName,
		[Email] = @Email,
		[NormalizedEmail] = @NormalizedEmail,
		[FirstName] = @FirstName,
		[LastName] = @LastName,
		[EmailConfirmed] = @EmailConfirmed,
		[PasswordHash] = @PasswordHash,
		[SecurityStamp] = @SecurityStamp,
		[ConcurrencyStamp] = @ConcurrencyStamp,
		[PhoneNumber] = @PhoneNumber,
		[PhoneNumberConfirmed] = @PhoneNumberConfirmed,
		[TwoFactorEnabled] = @TwoFactorEnabled,
		[LockoutEnd] = @LockoutEnd,
		[LockoutEnabled] = @LockoutEnabled,
		[AccessFailedCount] = @AccessFailedCount
	WHERE
		[Id] = @Id

	SET @Err = @@Error

	RETURN @Err
END
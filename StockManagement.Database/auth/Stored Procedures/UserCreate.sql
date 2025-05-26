-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Create a user record
-- ====================================================
CREATE PROCEDURE [auth].[UserCreate]
(
	@Id						INT OUTPUT,
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

	INSERT INTO [auth].[AspNetUsers]
	(
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
	)
	VALUES
	(
		@UserName,
		@NormalizedUserName,
		@Email,
		@NormalizedEmail,
		@FirstName,
		@LastName,
		@EmailConfirmed,	
		@PasswordHash,	
		@SecurityStamp,
		@ConcurrencyStamp,
		@PhoneNumber,
		@PhoneNumberConfirmed,
		@TwoFactorEnabled,
		@LockoutEnd,
		@LockoutEnabled,
		@AccessFailedCount
	)

	SELECT @Id = SCOPE_IDENTITY()

	SET @Err = @@Error

	RETURN @Err
END
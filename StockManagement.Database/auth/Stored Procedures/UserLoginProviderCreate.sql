-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Create a user login provider record
-- ====================================================
CREATE PROCEDURE [auth].[UserLoginProviderCreate]
(
	@LoginProvider			NVARCHAR(450),
	@ProviderKey			NVARCHAR(450),
	@ProviderDisplayName	NVARCHAR(MAX),
	@UserId					INT
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT INTO [auth].[AspNetUserLogins]
	(
		[LoginProvider],
		[ProviderKey],
		[ProviderDisplayName],
		[UserId]
	)
	VALUES
	(
		@LoginProvider,
		@ProviderKey,
		@ProviderDisplayName,
		@UserId
	)

	SET @Err = @@Error

	RETURN @Err
END
-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Create a role record
-- ====================================================
CREATE PROCEDURE [auth].[RoleCreate]
(
	@Id					INT OUTPUT,
	@Name				NVARCHAR(256),
	@NormalizedName		NVARCHAR(256),
	@ConcurrencyStamp	NVARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	INSERT INTO [auth].[AspNetRoles]
	(
		[Name],
		[NormalizedName],
		[ConcurrencyStamp]
	)
	VALUES
	(
		@Name,
		@NormalizedName,
		@ConcurrencyStamp
	)

	SELECT @Id = SCOPE_IDENTITY()

	SET @Err = @@Error

	RETURN @Err
END
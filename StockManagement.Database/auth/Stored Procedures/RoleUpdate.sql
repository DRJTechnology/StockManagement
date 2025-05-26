-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Update a role record
-- ====================================================
CREATE PROCEDURE [auth].[RoleUpdate]
(
	@Id					INT,
	@Name				NVARCHAR(256),
	@NormalizedName		NVARCHAR(256),
	@ConcurrencyStamp	NVARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	UPDATE [auth].[AspNetRoles]
	SET
		[Name] = @Name,
		[NormalizedName] = @NormalizedName,
		[ConcurrencyStamp] = @ConcurrencyStamp
	WHERE
		[Id] = @Id

	SET @Err = @@Error

	RETURN @Err
END
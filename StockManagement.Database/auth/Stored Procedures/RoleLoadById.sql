-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Get a role record by id
-- ====================================================
CREATE PROCEDURE [auth].[RoleLoadById]
(
	@Id	int
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[Name],
		[NormalizedName],
		[ConcurrencyStamp]
	FROM [auth].[AspNetRoles]
	WHERE
		([Id] = @Id)

	SET @Err = @@Error

	RETURN @Err
END
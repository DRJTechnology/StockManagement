-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Get a role record by name
-- ====================================================
CREATE PROCEDURE [auth].[RoleLoadByName]
(
	@NormalizedName nvarchar(256)
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
		([NormalizedName] = @NormalizedName)

	SET @Err = @@Error

	RETURN @Err
END
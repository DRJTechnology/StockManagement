-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	Get a role records by user id
-- ====================================================
CREATE PROCEDURE [auth].[RoleLoadByUserId]
(
	@UserId	int
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
	FROM [auth].[AspNetRoles] r
	INNER JOIN	[auth].[AspNetUserRoles] ur on r.Id = ur.RoleId
	WHERE
		(ur.[UserId] = @UserId)

	SET @Err = @@Error

	RETURN @Err
END
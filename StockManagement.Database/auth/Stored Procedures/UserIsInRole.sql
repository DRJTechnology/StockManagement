-- ====================================================
-- Author:		Dave Brown
-- Create date: 02 Jan 2024
-- Description:	return if user is in role or not
-- ====================================================
CREATE PROCEDURE [auth].[UserIsInRole]
	@Result		BIT OUTPUT,
	@UserId		INT,
	@RoleName	NVARCHAR(256)
AS
	IF EXISTS (
		SELECT 1 
		FROM [auth].[AspNetUserRoles] ur
		INNER JOIN [auth].[AspNetRoles] r ON ur.RoleId = r.Id
		WHERE	ur.UserId = @UserId
			AND	r.[NormalizedName] = @RoleName
		)
	BEGIN
		SET @Result = 1
	END
	ELSE
	BEGIN
		SET @Result = 0
	END

RETURN 0
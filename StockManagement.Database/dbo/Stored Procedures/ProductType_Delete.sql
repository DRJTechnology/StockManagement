-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete ProductType
-- =========================================================
CREATE PROCEDURE [dbo].[ProductType_Delete]
(
	@Success bit output,
	@ProductTypeID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [ProductType]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @ProductTypeID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
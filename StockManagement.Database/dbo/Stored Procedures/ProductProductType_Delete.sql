-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete ProductProductType
-- =========================================================
CREATE PROCEDURE [dbo].[ProductProductType_Delete]
(
	@Success bit output,
	@ProductProductTypeID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [ProductProductType]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @ProductProductTypeID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
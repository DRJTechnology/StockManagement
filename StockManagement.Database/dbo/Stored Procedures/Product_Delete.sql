-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Delete Product
-- =========================================================
CREATE PROCEDURE [dbo].[Product_Delete]
(
	@Success bit output,
	@ProductID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Product]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @ProductID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
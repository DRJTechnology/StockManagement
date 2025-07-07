-- =========================================================
-- Author:		Dave Brown
-- Create date: 07 JUL 2025
-- Description:	Delete Supplier
-- =========================================================
CREATE PROCEDURE [dbo].[Supplier_Delete]
(
	@Success bit output,
	@SupplierID int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Supplier]
	SET
		[Deleted] = 1,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @SupplierID

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

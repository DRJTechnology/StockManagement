-- =========================================================
-- Author:		Dave Brown
-- Create date: 07 JUL 2025
-- Description:	Update Supplier
-- =========================================================
CREATE PROCEDURE [dbo].[Supplier_Update]
(
	@Success bit output,
	@Id int,
	@SupplierName nvarchar(50),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Supplier]
	SET
		[SupplierName] = @SupplierName,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

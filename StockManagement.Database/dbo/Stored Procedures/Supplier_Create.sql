-- =========================================================
-- Author:		Dave Brown
-- Create date: 07 JUL 2025
-- Description:	Create Supplier
-- =========================================================
CREATE PROCEDURE [dbo].[Supplier_Create]
(
	@Success bit output,
	@Id int output,
	@SupplierName nvarchar(50),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Supplier] ([SupplierName],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@SupplierName, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

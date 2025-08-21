-- ===========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update ProductType
-- ===========================================================
-- 21 Aug 2025 - Dave Brown - Added Default Cost and Sale price
-- ===========================================================
CREATE PROCEDURE [dbo].[ProductType_Update]
(
	@Success bit output,
	@Id int,
	@ProductTypeName nvarchar(50),
	@DefaultCostPrice MONEY,
	@DefaultSalePrice MONEY,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [ProductType]
	SET
		[ProductTypeName] = @ProductTypeName,
		[DefaultCostPrice] = @DefaultCostPrice,
		[DefaultSalePrice] = @DefaultSalePrice,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
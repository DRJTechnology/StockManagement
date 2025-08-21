-- ===========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Product Type
-- ===========================================================
-- 21 Aug 2025 - Dave Brown - Added Default Cost and Sale price
-- ===========================================================
CREATE PROCEDURE [dbo].[ProductType_Create]
(
	@Success bit output,
	@Id int output,
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

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[ProductType] ([ProductTypeName],[DefaultCostPrice],[DefaultSalePrice],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@ProductTypeName, @DefaultCostPrice, @DefaultSalePrice, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
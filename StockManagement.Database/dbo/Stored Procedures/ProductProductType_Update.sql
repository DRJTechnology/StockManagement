-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update ProductProductType
-- =========================================================
CREATE PROCEDURE [dbo].[ProductProductType_Update]
(
	@Success bit output,
	@Id int,
	@ProductId int,
	@ProductTypeId int,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [ProductProductType]
	SET
		[ProductId] = @ProductId,
		[ProductTypeId] = @ProductTypeId,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2025
-- Description:	Update ProductType
-- =========================================================
CREATE PROCEDURE [dbo].[ProductType_Update]
(
	@Success bit output,
	@Id int,
	@ProductTypeName nvarchar(50),
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
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
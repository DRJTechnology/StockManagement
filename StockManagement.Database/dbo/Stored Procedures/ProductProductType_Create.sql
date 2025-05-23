-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create ProductProductType
-- =========================================================
CREATE PROCEDURE [dbo].[ProductProductType_Create]
(
	@Success bit output,
	@Id int output,
	@ProductId int,
	@ProductTypeId int,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[ProductProductType] ([ProductId],[ProductTypeId],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@ProductId, @ProductTypeId, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
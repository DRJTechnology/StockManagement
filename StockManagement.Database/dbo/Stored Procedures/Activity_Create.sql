-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Activity
-- =========================================================
-- 30 Aug 2025 - Dave Brown - Added Notes
-- =========================================================
CREATE PROCEDURE [dbo].[Activity_Create]
(
	@Success bit output,
	@Id int output,
	@ActivityDate datetime,
	@ActionId int,
	@ProductId int,
	@ProductTypeId int,
	@LocationId int,
	@Quantity int,
	@Notes nvarchar(1024) = NULL,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[LocationId],[Quantity],[Notes],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@ActivityDate, @ActionId,@ProductId, @ProductTypeId,@LocationId, @Quantity, @Notes, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
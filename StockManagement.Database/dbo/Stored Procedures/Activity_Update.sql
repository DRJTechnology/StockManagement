-- =========================================================
-- Author:		Dave Brown
-- Create date: 27 May 2025
-- Description:	Update Activity
-- =========================================================
-- 30 Aug 2025 - Dave Brown - Added Notes
-- =========================================================
CREATE PROCEDURE [dbo].[Activity_Update]
(
	@Success bit output,
	@Id int,
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
	SET @Success = 0

	UPDATE [Activity]
	SET
		[ActivityDate] = @ActivityDate,
		[ActionId] = @ActionId,
		[ProductId] = @ProductId,
		[ProductTypeId] = @ProductTypeId,
		[LocationId] = @LocationId,
		[Quantity] = @Quantity,
		[Notes] = @Notes,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
-- =========================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Create Stock Order Detail
-- =========================================================
CREATE PROCEDURE [dbo].[StockOrderDetail_Create]
(
	@Success bit output,
	@Id int output,
	@StockOrderId int,
	@ProductId int,
	@ProductTypeId int,
	@Quantity int,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate		DATETIME
	DECLARE @ActivityDate	DATETIME
	DECLARE @ActionId		INT = 1 -- Add new stock add to
	SET @UpdateDate = GetDate()

	SELECT	@ActivityDate = sr.[Date]
	FROM	dbo.[StockOrder] sr
	WHERE	sr.Id = @StockOrderId
	
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO dbo.[StockOrderDetail] ([StockOrderId],[ProductId],[ProductTypeId],[Quantity],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@StockOrderId, @ProductId, @ProductTypeId, @Quantity, @Deleted, @CurrentUserId, @UpdateDate)

		SELECT @ID = SCOPE_IDENTITY()

		-- Create the activity record to add stock to the stock room
		INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[LocationId],[Quantity],[StockOrderDetailId],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@ActivityDate, @ActionId, @ProductId, @ProductTypeId, 1, @Quantity, @ID, @Deleted, @CurrentUserId, @UpdateDate)

		COMMIT TRANSACTION
		SET @Success = 1
		SET @Err = 0
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION

		SET @Success = 0
		SET @Err = ERROR_NUMBER()
	END CATCH

	RETURN @Err
END
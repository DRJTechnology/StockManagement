-- =========================================================
-- Author:		Dave Brown
-- Create date: 03 Jul 2025
-- Description:	Create Delivery Note Detail
-- =========================================================
CREATE PROCEDURE [dbo].[StockSaleDetail_Create]
(
	@Success bit output,
	@Id int output,
	@StockSaleId int,
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
	DECLARE @ActionId		INT = 2 -- Move from stock room to
	DECLARE @LocationId		INT
	--DECLARE @DirectSale		BIT
	SET @UpdateDate = GetDate()

	SELECT	@ActivityDate = dn.[Date],
			@LocationId = dn.[LocationId]--,
			--@DirectSale = dn.[DirectSale]
	FROM	dbo.[StockSale] dn
	WHERE	dn.Id = @StockSaleId
	
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO dbo.[StockSaleDetail] ([StockSaleId],[ProductId],[ProductTypeId],[Quantity],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@StockSaleId, @ProductId, @ProductTypeId, @Quantity, @Deleted, @CurrentUserId, @UpdateDate)

		SELECT @Id = SCOPE_IDENTITY()

		-- Create the activity record to move from the stock room to the location
		INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[LocationId],[Quantity],[StockSaleDetailId],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@ActivityDate, @ActionId, @ProductId, @ProductTypeId, @LocationId, @Quantity, @ID, @Deleted, @CurrentUserId, @UpdateDate)

		---- If this is a direct sale, sell from the location
		--IF @DirectSale = 1
		--BEGIN
		--	SET @ActionId = 5 -- Sell from location
		--	INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[LocationId],[Quantity],[StockSaleDetailId],[Deleted],[AmendUserID],[AmendDate])
		--	VALUES (@ActivityDate, @ActionId, @ProductId, @ProductTypeId, @LocationId, @Quantity, @ID, @Deleted, @CurrentUserId, @UpdateDate)
		--END

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
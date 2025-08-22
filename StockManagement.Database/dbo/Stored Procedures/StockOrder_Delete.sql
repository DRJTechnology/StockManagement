-- =========================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Delete Stock Order
-- =========================================================
CREATE PROCEDURE [dbo].[StockOrder_Delete]
(
	@Success bit output,
	@StockOrderId int,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	DECLARE @UpdateDate		DATETIME
	SET @UpdateDate = GetDate()

	BEGIN TRY
		BEGIN TRANSACTION

		-- Soft delete the stock order
		UPDATE [StockOrder]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE
			[Id] = @StockOrderId

		-- Soft delete the stock order details
		UPDATE [StockOrderDetail]
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE	[StockOrderId] = @StockOrderId
			AND [Deleted] = 0

		-- Soft delete the activities associated with the stock order details
		UPDATE a
		SET
			[Deleted] = 1,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		FROM
			[Activity] a
			INNER JOIN [StockOrderDetail] srd ON a.[StockOrderDetailId] = srd.[Id]
		WHERE	srd.[StockOrderId] = @StockOrderId
			AND a.[Deleted] = 0

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
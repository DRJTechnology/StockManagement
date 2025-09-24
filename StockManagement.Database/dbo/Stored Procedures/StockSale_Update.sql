-- =========================================================
-- Author:		Dave Brown
-- Create date: 02 Jul 2025
-- Description:	Update Delivery Note
-- =========================================================
CREATE PROCEDURE [dbo].[StockSale_Update]
(
	@Success bit output,
	@Id int,
	@Date datetime,
	@LocationId int,
	@ContactId int,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	DECLARE @UpdateDate			DATETIME
	--DECLARE @InitialDirectSale	BIT
	SET @UpdateDate = GetDate()

	--SELECT	@InitialDirectSale = dn.[DirectSale]
	--FROM	dbo.[StockSale] dn
	--WHERE	dn.Id = @Id

	BEGIN TRY
		BEGIN TRANSACTION

		-- Update the delivery note
		UPDATE [StockSale]
		SET
			[Date] = @Date,
			[LocationId] = @LocationId,
			[ContactId] = @ContactId,
			[Deleted] = @Deleted,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		WHERE	[Id] = @Id
			AND [Deleted] = 0


		-- Update affected activity records
		UPDATE a
		SET
			[ActivityDate] = @Date,
			[LocationId] = @LocationId,
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		FROM [Activity] a
		INNER JOIN [StockSaleDetail] dnd ON a.StockSaleDetailId = dnd.Id
		WHERE	a.Deleted = 0
			AND dnd.StockSaleId = @Id


		--IF @InitialDirectSale = 0 AND @DirectSale = 1
		--BEGIN -- If the direct sale is being set to true, we need to add 'sell from' activity records
		--	INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[LocationId],[Quantity],[StockSaleDetailId],[Deleted],[AmendUserID],[AmendDate])
		--	SELECT	@Date, 5, a.ProductId, a.ProductTypeId, @LocationId, a.Quantity, dnd.Id, 0, @CurrentUserId, @UpdateDate
		--	FROM	dbo.[Activity] a
		--	INNER JOIN [StockSaleDetail] dnd ON a.StockSaleDetailId = dnd.Id
		--	WHERE	a.Deleted = 0
		--		AND a.ActionId = 2 -- Move from stock room to location
		--		AND dnd.StockSaleId = @Id
		--END
		--ELSE IF @InitialDirectSale = 1 AND @DirectSale = 0
		--BEGIN -- If the direct sale is being set to false, we need to remove 'sell from' activity records
		--	UPDATE a
		--	SET
		--		[Deleted] = 1,
		--		[AmendUserID] = @CurrentUserId,
		--		[AmendDate] = @UpdateDate
		--	FROM [Activity] a
		--	INNER JOIN [StockSaleDetail] dnd ON a.StockSaleDetailId = dnd.Id
		--	WHERE	a.Deleted = 0
		--		AND a.ActionId = 5 -- Sell from location
		--		AND dnd.StockSaleId = @Id
		--END

		COMMIT TRANSACTION
		SET @Success = 1
		SET @Err = 0
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		
		INSERT INTO dbo.ErrorLog (ErrorDate,	ProcedureName, ErrorNumber, ErrorSeverity, ErrorState, ErrorLine, ErrorMessage, UserId)
		VALUES (GETDATE(), ERROR_PROCEDURE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_MESSAGE(), @CurrentUserId);

		SET @Success = 0
		SET @Err = ERROR_NUMBER()
	END CATCH

	RETURN @Err
END
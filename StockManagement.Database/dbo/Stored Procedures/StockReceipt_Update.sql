-- =========================================================
-- Author:		Dave Brown
-- Create date: 02 Jul 2025
-- Description:	Update Stock Receipt
-- =========================================================
CREATE PROCEDURE [dbo].[StockReceipt_Update]
(
	@Success bit output,
	@Id int,
	@Date datetime,
	@ContactId int,
	@DirectSale bit,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	DECLARE @UpdateDate			DATETIME
	SET @UpdateDate = GetDate()

	BEGIN TRY
		BEGIN TRANSACTION

		-- Update the stock receipt
		UPDATE [StockReceipt]
		SET
			[Date] = @Date,
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
			--[LocationId] = location will be stock room
			[AmendUserID] = @CurrentUserId,
			[AmendDate] = @UpdateDate
		FROM [Activity] a
		INNER JOIN [StockReceiptDetail] srd ON a.StockReceiptDetailId = srd.Id
		WHERE	a.Deleted = 0
			AND srd.StockReceiptId = @Id

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
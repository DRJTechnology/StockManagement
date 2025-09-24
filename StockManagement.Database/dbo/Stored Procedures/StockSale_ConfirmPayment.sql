-- ==================================================================
-- Author:		Dave Brown
-- Create date: 14 Sep 2025
-- Description:	Confirm payment of stock sale
-- ==================================================================
CREATE PROCEDURE [dbo].[StockSale_ConfirmPayment]
(
	@Success bit output,
	@StockSaleId int,
	@PaymentDate datetime,
	@Description nvarchar(512),
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME,
			@TransactionId INT,
			@TransactionDetailId INT,
			@TotalPrice MONEY,
			@ContactId INT
	SET		@UpdateDate = GetDate()

	SELECT	@TotalPrice = TotalPrice,
			@ContactId = ContactId
	FROM	dbo.StockSale
	WHERE	Id = @StockSaleId

	BEGIN TRY
		BEGIN TRANSACTION

		EXEC [finance].[Transaction_CreateExpenseIncome]
			@Success = @Success OUTPUT,
			@TransactionDetailId = @TransactionDetailId OUTPUT,
			@TransactionId = @TransactionId OUTPUT,
			@TransactionTypeId = 3,
			@AccountId = 110, -- Sales - Art
			@Date = @PaymentDate,
			@Description = @Description,
			@Amount = @TotalPrice,
			@ContactId = @ContactId,
			@CurrentUserId = @CurrentUserId


		-- Update the stock sale PaymentReceived
		UPDATE dbo.StockSale
		SET	TransactionId = @TransactionId,
			PaymentReceived = 1,
			AmendUserID = @CurrentUserId,
			AmendDate = @UpdateDate
		WHERE Id = @StockSaleId

		COMMIT TRANSACTION

		SET @Success = 1
		SET @Err = 0
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		
		INSERT INTO dbo.ErrorLog
		(ErrorDate,	ProcedureName, ErrorNumber, ErrorSeverity, ErrorState, ErrorLine, ErrorMessage, UserId)
		VALUES (GETDATE(), ERROR_PROCEDURE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_LINE(), ERROR_MESSAGE(), @CurrentUserId);

		SET @Success = 0
		SET @Err = ERROR_NUMBER()
	END CATCH

	RETURN @Err
END
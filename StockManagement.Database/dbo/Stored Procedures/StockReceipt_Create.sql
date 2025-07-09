-- =========================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Get Stock Receipts
-- =========================================================
CREATE PROCEDURE [dbo].[StockReceipt_Create]
(
	@Success bit output,
	@Id int output,
	@Date datetime,
	@SupplierId int,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[StockReceipt] ([Date],[SupplierId],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@Date, @SupplierId, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
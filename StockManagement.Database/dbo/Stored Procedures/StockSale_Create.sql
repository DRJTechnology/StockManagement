-- =========================================================
-- Author:		Dave Brown
-- Create date: 02 Jul 2025
-- Description:	Get Delivery Notes
-- =========================================================
CREATE PROCEDURE [dbo].[StockSale_Create]
(
	@Success bit output,
	@Id int output,
	@Date datetime,
	@LocationId int,
	--@DirectSale bit,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[StockSale] ([Date],[LocationId], /* [DirectSale], */ [Deleted],[AmendUserID],[AmendDate])
	VALUES (@Date, @LocationId, /* @DirectSale, */ @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @Id = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
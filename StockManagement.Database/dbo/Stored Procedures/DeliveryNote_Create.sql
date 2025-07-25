-- =========================================================
-- Author:		Dave Brown
-- Create date: 02 Jul 2025
-- Description:	Get Delivery Notes
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNote_Create]
(
	@Success bit output,
	@Id int output,
	@Date datetime,
	@LocationId int,
	@DirectSale bit,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[DeliveryNote] ([Date],[LocationId],[DirectSale],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@Date, @LocationId, @DirectSale, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
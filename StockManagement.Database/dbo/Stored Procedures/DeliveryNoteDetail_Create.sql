-- =========================================================
-- Author:		Dave Brown
-- Create date: 03 Jul 2025
-- Description:	Create Delivery Note Detail
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNoteDetail_Create]
(
	@Success bit output,
	@Id int output,
	@DeliveryNoteId int,
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
	DECLARE @VenueId		INT
	DECLARE @DirectSale		BIT
	SET @UpdateDate = GetDate()

	SELECT	@ActivityDate = dn.[Date],
			@VenueId = dn.[VenueId],
			@DirectSale = dn.[DirectSale]
	FROM	dbo.[DeliveryNote] dn
	WHERE	dn.Id = @DeliveryNoteId
	
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO dbo.[DeliveryNoteDetail] ([DeliveryNoteId],[ProductId],[ProductTypeId],[Quantity],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@DeliveryNoteId, @ProductId, @ProductTypeId, @Quantity, @Deleted, @CurrentUserId, @UpdateDate)

		SELECT @ID = SCOPE_IDENTITY()

		-- Create the activity record to move from the stock room to the venue
		INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[VenueId],[Quantity],[DeliveryNoteDetailId],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@ActivityDate, @ActionId, @ProductId, @ProductTypeId, @VenueId, @Quantity, @ID, @Deleted, @CurrentUserId, @UpdateDate)

		-- If this is a direct sale, sell from the venue
		IF @DirectSale = 1
		BEGIN
			SET @ActionId = 5 -- Sell from venue
			INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[VenueId],[Quantity],[DeliveryNoteDetailId],[Deleted],[AmendUserID],[AmendDate])
			VALUES (@ActivityDate, @ActionId, @ProductId, @ProductTypeId, @VenueId, @Quantity, @ID, @Deleted, @CurrentUserId, @UpdateDate)
		END

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
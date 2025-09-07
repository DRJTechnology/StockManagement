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
	DECLARE @LocationId		INT
	SET @UpdateDate = GetDate()

	SELECT	@ActivityDate = dn.[Date],
			@LocationId = dn.[LocationId]--,
	FROM	dbo.[DeliveryNote] dn
	WHERE	dn.Id = @DeliveryNoteId
	
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO dbo.[DeliveryNoteDetail] ([DeliveryNoteId],[ProductId],[ProductTypeId],[Quantity],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@DeliveryNoteId, @ProductId, @ProductTypeId, @Quantity, @Deleted, @CurrentUserId, @UpdateDate)

		SELECT @ID = SCOPE_IDENTITY()

		-- Create the activity record to move from the stock room to the location
		INSERT INTO dbo.[Activity] ([ActivityDate],[ActionId],[ProductId],[ProductTypeId],[LocationId],[Quantity],[DeliveryNoteDetailId],[Deleted],[AmendUserID],[AmendDate])
		VALUES (@ActivityDate, @ActionId, @ProductId, @ProductTypeId, @LocationId, @Quantity, @ID, @Deleted, @CurrentUserId, @UpdateDate)

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
-- =========================================================
-- Author:		Dave Brown
-- Create date: 02 Jul 2025
-- Description:	Update Delivery Note
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNote_Update]
(
	@Success bit output,
	@Id int,
	@Date datetime,
	@LocationId int,
	@DeliveryCompleted bit,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	DECLARE @UpdateDate			DATETIME
	DECLARE @InitialDeliveryConfirmed	BIT
	SET @UpdateDate = GetDate()

	SELECT	@InitialDeliveryConfirmed = dn.[DeliveryCompleted]
	FROM	dbo.[DeliveryNote] dn
	WHERE	dn.Id = @Id

	BEGIN TRY
		BEGIN TRANSACTION

		-- Update the delivery note
		UPDATE [DeliveryNote]
		SET
			[Date] = @Date,
			[LocationId] = @LocationId,
			[DeliveryCompleted] = @DeliveryCompleted,
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
		INNER JOIN [DeliveryNoteDetail] dnd ON a.DeliveryNoteDetailId = dnd.Id
		WHERE	a.Deleted = 0
			AND dnd.DeliveryNoteId = @Id


		IF @InitialDeliveryConfirmed = 0 AND @DeliveryCompleted = 1
		BEGIN -- If the delivery note is being confirmed, use InventoryBatch_MoveQuantity to move each of the DeliveryNoteDetail records
			DECLARE @DeliveryNoteDetailId int
			DECLARE @ProductId int
			DECLARE @ProductTypeId int
			DECLARE @Quantity int
			DECLARE @FromLocationId int = 1 -- Default from location to Stock Room
			DECLARE @ActivityId int

			DECLARE DeliveryNoteDetailCursor CURSOR FOR
			SELECT	dnd.Id, dnd.ProductId, dnd.ProductTypeId, dnd.Quantity
			FROM	[dbo].[DeliveryNoteDetail] dnd
			INNER JOIN [dbo].[DeliveryNote] dn ON dnd.DeliveryNoteId = dn.Id
			INNER JOIN [dbo].[Location] l ON dn.LocationId = l.Id
			WHERE	dn.Id = @Id
				AND dnd.Deleted = 0

			OPEN DeliveryNoteDetailCursor
			FETCH NEXT FROM DeliveryNoteDetailCursor INTO @DeliveryNoteDetailId, @ProductId, @ProductTypeId, @Quantity
			WHILE @@FETCH_STATUS = 0
			BEGIN
				DECLARE @MoveErr int

				SELECT	@ActivityId = Id
				FROM	[Activity] a
				WHERE	a.DeliveryNoteDetailId = @DeliveryNoteDetailId
					
				EXEC	@MoveErr = [finance].[InventoryBatch_MoveQuantity]
						@ProductId = @ProductId,
						@ProductTypeId = @ProductTypeId,
						@FromLocationId = @FromLocationId,
						@ToLocationId = @LocationId,
						@Quantity = @Quantity,
						@ActivityId = @ActivityId,
						@UserId = @CurrentUserId

				IF @MoveErr <> 0
				BEGIN
					-- Error moving inventory batch quantity - raise error to trigger rollback of transaction
					RAISERROR('Error moving inventory batch quantity for DeliveryNoteDetail Id %d', 16, 1, @DeliveryNoteDetailId)
					SET @Err = -1
				END
				FETCH NEXT FROM DeliveryNoteDetailCursor INTO @DeliveryNoteDetailId, @ProductId,@ProductTypeId, @Quantity
			END
			CLOSE DeliveryNoteDetailCursor
			DEALLOCATE DeliveryNoteDetailCursor
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
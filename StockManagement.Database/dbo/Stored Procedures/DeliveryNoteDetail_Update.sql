﻿-- =========================================================
-- Author:		Dave Brown
-- Create date: 03 Jul 2025
-- Description:	Update Delivery Note Detail
-- =========================================================
CREATE PROCEDURE [dbo].[DeliveryNoteDetail_Update]
(
	@Success bit output,
	@Id int,
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
	SET @Success = 0

	DECLARE @UpdateDate		DATETIME
	SET @UpdateDate = GetDate()

	UPDATE [DeliveryNoteDetail]
	SET
		[ProductId] = @ProductId,
		[ProductTypeId] = @ProductTypeId,
		[Quantity] = @Quantity,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = @UpdateDate
	WHERE
		[Id] = @Id

	UPDATE [Activity]
	SET
		[ProductId] = @ProductId,
		[ProductTypeId] = @ProductTypeId,
		[Quantity] = @Quantity,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = @UpdateDate
	WHERE
		[DeliveryNoteDetailId] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

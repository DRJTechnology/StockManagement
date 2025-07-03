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

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[DeliveryNoteDetail] ([DeliveryNoteId],[ProductId],[ProductTypeId],[Quantity],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@DeliveryNoteId, @ProductId, @ProductTypeId, @Quantity, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
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
	@VenueId int,
	@DirectSale bit,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [DeliveryNote]
	SET
		[Date] = @Date,
		[VenueId] = @VenueId,
		[DirectSale] = @DirectSale,
		[Deleted] = @Deleted,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END

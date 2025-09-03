-- =========================================================
-- Author:		Dave Brown
-- Create date: 08 Jul 2025
-- Description:	Get Stock Orders
-- =========================================================
CREATE PROCEDURE [dbo].[StockOrder_Create]
(
	@Success bit output,
	@Id int output,
	@Date datetime,
	@ContactId int,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[StockOrder] ([Date],[ContactId],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@Date, @ContactId, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @Id = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
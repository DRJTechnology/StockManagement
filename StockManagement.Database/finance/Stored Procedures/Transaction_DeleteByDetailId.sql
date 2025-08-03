-- ============================================================================
-- Author:		Dave Brown
-- Create date: 03 Aug 2025
-- Description:	Updates a transaction and transaction detail records to deleted
-- ============================================================================
CREATE PROCEDURE [finance].[Transaction_DeleteByDetailId]
	@Success				BIT OUTPUT,
	@TransactionDetailId	INT, 
	@CurrentUserId			INT
AS
BEGIN
	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	DECLARE @TransactionId			INT
	
	SELECT	@TransactionId = TransactionId
	FROM	finance.[TransactionDetail]
	Where	Id = @TransactionDetailId
		AND Deleted = 0

	UPDATE	[finance].[Transaction]
	SET		Deleted = 1,
			AmendUserId = @CurrentUserId,
			AmendDate = SYSDATETIME()
	WHERE	Id = @TransactionId
		AND Deleted = 0

	UPDATE	[finance].[TransactionDetail]
	SET		Deleted = 1,
			AmendUserId = @CurrentUserId,
			AmendDate = SYSDATETIME()
	WHERE	TransactionId = @TransactionId
		AND Deleted = 0

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err

END
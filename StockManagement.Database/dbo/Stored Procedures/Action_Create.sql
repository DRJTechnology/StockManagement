-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Action
-- =========================================================
CREATE PROCEDURE [dbo].[Action_Create]
(
	@Success bit output,
	@Id int output,
	@ActionName nvarchar(50),
	@Direction int,
	@AffectStockRoom bit,
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Action] ([ActionName],[Direction],[AffectStockRoom],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@ActionName, @Direction, @AffectStockRoom, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @ID = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
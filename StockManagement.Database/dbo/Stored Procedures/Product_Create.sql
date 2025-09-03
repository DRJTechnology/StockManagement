-- =========================================================
-- Author:		Dave Brown
-- Create date: 23 May 2024
-- Description:	Create Product
-- =========================================================
CREATE PROCEDURE [dbo].[Product_Create]
(
	@Success bit output,
	@Id int output,
	@ProductName nvarchar(50),
	@Deleted bit,
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int

	DECLARE @UpdateDate DATETIME
	SET @UpdateDate = GetDate()

	INSERT INTO dbo.[Product] ([ProductName],[Deleted],[AmendUserID],[AmendDate])
	VALUES (@ProductName, @Deleted, @CurrentUserId, @UpdateDate)

	SELECT @Id = SCOPE_IDENTITY()

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
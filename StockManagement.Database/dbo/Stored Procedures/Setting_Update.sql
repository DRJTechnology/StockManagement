-- =========================================================
-- Author:		Dave Brown
-- Create date: 18 Jul 2025
-- Description:	Update Setting
-- =========================================================
CREATE PROCEDURE [dbo].[Setting_Update]
(
	@Success bit output,
	@Id int,
	@SettingValue nvarchar(255),
	@CurrentUserId int
)
AS
BEGIN

	SET NOCOUNT OFF
	DECLARE @Err int
	SET @Success = 0

	UPDATE [Setting]
	SET
		[SettingValue] = @SettingValue,
		[AmendUserID] = @CurrentUserId,
		[AmendDate] = GetDate()
	WHERE
		[Id] = @Id

	SET @Success = 1

	SET @Err = @@Error

	RETURN @Err
END
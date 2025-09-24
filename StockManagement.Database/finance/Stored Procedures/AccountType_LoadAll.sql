-- =========================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Get all account type records
-- =========================================
CREATE PROCEDURE [finance].[AccountType_LoadAll]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	Id, [Type]
	FROM	[finance].[AccountType]
	Where	Deleted = 0
	ORDER BY [Type]

END
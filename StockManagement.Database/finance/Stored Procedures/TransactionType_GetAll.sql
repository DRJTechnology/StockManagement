-- =========================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Get all transaction type records
-- =========================================
CREATE PROCEDURE [finance].[TransactionType_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	Id, [Type]
	FROM	[finance].[TransactionType]
	Where	Deleted = 0
	ORDER BY [Type]

END
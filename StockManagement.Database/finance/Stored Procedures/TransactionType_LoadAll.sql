-- ================================================
-- Author:		Dave Brown
-- Create date: 22 Jul 2025
-- Description:	Load all transaction type records
-- ================================================
CREATE PROCEDURE [finance].[TransactionType_LoadAll]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	Id, [Type]
	FROM	[finance].[TransactionType]
	Where	Deleted = 0
	ORDER BY [Type]

END
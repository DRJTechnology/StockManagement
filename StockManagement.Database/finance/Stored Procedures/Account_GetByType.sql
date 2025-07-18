-- =============================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Get all account records by type
-- =============================================
CREATE PROCEDURE [finance].[Account_GetByType]
	@AccountTypeId	INT,
	@ActiveOnly		  BIT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	a.Id, AccountTypeId, [Name], act.[Type], a.Notes, a.Active, a.Deleted, a.CreateUserId, a.CreateDate, a.AmendUserId, a.AmendDate
	FROM	[finance].[Account] a
	INNER JOIN [finance].[AccountType] act ON a.AccountTypeId = act.Id
	Where	a.Deleted = 0
    AND a.AccountTypeId = @AccountTypeId
		AND (@ActiveOnly != 1 OR a.Active = 1)
	ORDER BY [Name]

END
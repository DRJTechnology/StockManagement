-- ====================================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Get an account record
-- ====================================================
CREATE PROCEDURE [finance].[Account_LoadById]
	@Id					INT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	a.Id, AccountTypeId, [Name], act.[Type], a.Notes, a.Active, a.Deleted, a.CreateUserId, a.CreateDate, a.AmendUserId, a.AmendDate
	FROM	[finance].[Account] a
	INNER JOIN [finance].[AccountType] act ON a.AccountTypeId = act.Id
	WHERE	a.Id = @Id

END
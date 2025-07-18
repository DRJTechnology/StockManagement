-- ==========================================================
-- Author:		Dave Brown
-- Create date: 11 Jul 2025
-- Description:	Creates an account record
-- ==========================================================
CREATE PROCEDURE [finance].[BankStatement_Upload]
	@BankAccountId  INT,
	@PortfolioId    INT,
  @Statement finance.StatementTableType READONLY,
  @CurrentUserId	INT
AS
	SET NOCOUNT ON;

  DECLARE @TotalRowCount  INT
  DECLARE @InsertedRowCount  INT

  SELECT @TotalRowCount = COUNT(1) FROM @Statement

  DECLARE @UploadId UNIQUEIDENTIFIER
  SET @UploadId = NEWID()

  INSERT INTO [finance].[BankAccountDetail] (BankAccountId, UploadId, Date, Amount, [Description], TransactionType, Deleted, CreateUserId, CreateDate, AmendUserId, AmendDate)
  SELECT  @BankAccountId, @UploadId, s.Date, s.Amount, REPLACE(s.Description, CHAR(9), ''), s.TransactionType, 0, @CurrentUserId, SYSDATETIME(), @CurrentUserId, SYSDATETIME()
  FROM        @Statement s
  LEFT OUTER JOIN  [finance].[BankAccountDetail] bad ON bad.BankAccountId = @BankAccountId AND s.Amount = bad.Amount AND s.[Date] = bad.[Date] AND REPLACE(s.Description, CHAR(9), '') = REPLACE(bad.Description, CHAR(9), '') AND s.TransactionType = bad.TransactionType
  WHERE bad.Id IS NULL

  SELECT @InsertedRowCount = @@ROWCOUNT

  SELECT  @TotalRowCount AS TotalRowCount, @InsertedRowCount AS InsertedRowCount

RETURN 0
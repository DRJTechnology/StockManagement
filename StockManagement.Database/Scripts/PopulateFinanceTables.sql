
-- Populate the TransactionType table with initial data
INSERT INTO [finance].[TransactionType] ([Id],[Type],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (1,'Journal',0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[TransactionType] ([Id],[Type],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (2,'Expense',0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[TransactionType] ([Id],[Type],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (3,'Income',0,0,GETDATE(),0,GETDATE())
GO

-- Populate the AccountType table with initial data
INSERT INTO [finance].[AccountType] ([Id],[Type],[CreditDebit],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (1,'Asset',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[AccountType] ([Id],[Type],[CreditDebit],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (2,'Liability',-1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[AccountType] ([Id],[Type],[CreditDebit],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (3,'Revenue',-1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[AccountType] ([Id],[Type],[CreditDebit],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (4,'Expense',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[AccountType] ([Id],[Type],[CreditDebit],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (5,'Equity',-1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[AccountType] ([Id],[Type],[CreditDebit],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (6,'Long-term Liability',-1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[AccountType] ([Id],[Type],[CreditDebit],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (7,'Bank\Cash Account',1,0,0,GETDATE(),0,GETDATE())
GO


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
GO

-- Populate the Account table with initial data
INSERT INTO [finance].[Account] ([AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (2,'Accounts Payable','Current liabilities',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] ([AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (1,'Accounts Receivable','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] ([AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (5,'Owner’s Investment/Drawings','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] ([AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (3,'Other Income','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] ([AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (4,'Advertising & Promotion','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] ([AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (4,'Cost of Goods Sold','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] ([AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (3,'Sales','',1,0,0,GETDATE(),0,GETDATE())

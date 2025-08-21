
-- UPDATE THE SCHEMA FIRST

-- Populate the TransactionType table with initial data
INSERT INTO [finance].[TransactionType] ([Id],[Type],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (1,'Journal',0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[TransactionType] ([Id],[Type],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (2,'Expense',0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[TransactionType] ([Id],[Type],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (3,'Income',0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[TransactionType] ([Id],[Type],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (4,'Sale',0,0,GETDATE(),0,GETDATE())
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
SET IDENTITY_INSERT [finance].[Account] ON;
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (1,2,'Accounts Payable','Current liabilities',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (2,1,'Accounts Receivable','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (3,5,'Owner’s Capital/Investment','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (4,5,'Owner’s Drawings','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (5,3,'Other Income','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (6,1,'Inventory','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (7,4,'Inventory Rounding Differences','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (8,4,'Advertising & Promotion','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (9,4,'Cost of Goods Sold','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (10,3,'Sales - Art','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (11,3,'Sales - Skincare','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (12,4,'Subscriptions & Memberships','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (13,4,'Exhibitions & Competitions','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (14,4,'Stationary & Consumables','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (15,4,'Charity Donation','',1,0,0,GETDATE(),0,GETDATE())
SET IDENTITY_INSERT [finance].[Account] OFF;


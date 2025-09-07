
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
VALUES (10,4,'Inventory Shrinkage','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (110,3,'Sales - Art','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (111,3,'Sales - Skincare','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (112,4,'Subscriptions & Memberships','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (113,4,'Exhibitions & Competitions','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (114,4,'Stationary & Consumables','',1,0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[Account] (Id,[AccountTypeId],[Name],[Notes],[Active],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (115,4,'Charity Donation','',1,0,0,GETDATE(),0,GETDATE())
SET IDENTITY_INSERT [finance].[Account] OFF;

-- Populate the InventoryBatchStatus table with initial data
INSERT INTO [finance].[InventoryBatchStatus] ([Id],[Status],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (1,'Pending',0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[InventoryBatchStatus] ([Id],[Status],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (2,'Active',0,0,GETDATE(),0,GETDATE())
INSERT INTO [finance].[InventoryBatchStatus] ([Id],[Status],[Deleted],[CreateUserId],[CreateDate],[AmendUserId],[AmendDate])
VALUES (3,'Depleted',0,0,GETDATE(),0,GETDATE())

-- If Action table doesn't have id=8 than add it
IF NOT EXISTS (SELECT 1 FROM [dbo].[Action] WHERE Id = 8)
BEGIN
	INSERT INTO [dbo].[Action] ([Id],[ActionName],[Direction],[AffectStockRoom],[Deleted],[AmendUserID],[AmendDate])
	VALUES (8, 'Personal use', -1, 0, 0, 1, GETDATE())
END
GO

-- Update all DeliveryNote records so that the DeliveyCompleted value is true
UPDATE	dbo.DeliveryNote
SET		DeliveryCompleted = 1
WHERE	Deleted = 0
GO

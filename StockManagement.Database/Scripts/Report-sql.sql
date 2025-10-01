
--Display all transactions in a human-readable way:
SELECT	td.TransactionId,
		tt.[Type],
		td.[Date], 
		t.Reference,
		a.[Name] AS Account, 
		td.[Description],
		CASE WHEN direction = 1 THEN amount END AS DR, 
		CASE WHEN direction = -1 THEN amount END AS CR
FROM	finance.TransactionDetail td
INNER JOIN	finance.[Transaction] t ON td.TransactionId = t.Id
INNER JOIN	finance.[TransactionType] tt ON t.TransactionTypeId = tt.Id
INNER JOIN	finance.Account a on td.AccountId = a.Id
-- where td.AccountId = 3 -- Owner’s Investment/Drawings
order by	td.TransactionId, a.[Name], CR, DR

--Balance sheet to date:
DECLARE	@ToDate	DaTETIME
SET @ToDate	= '01 Nov 2025'
SELECT	act.Id, a.Id, act.[Type], a.[Name], sum(td.Amount * td.Direction * act.CreditDebit) as balance
FROM	finance.TransactionDetail td
INNER JOIN finance.Account a on td.AccountId = a.Id
INNER JOIN finance.AccountType act on a.AccountTypeId = act.Id
WHERE td.Date < @ToDate
group by	act.Id, a.Id, act.[Type], a.[Name]
order by	act.[Type], a.[Name]

-- Total value of stock in InventoryBatch records
SELECT	SUM(Quantityremaining * UnitCost) AS TotalValue
FROM	finance.InventoryBatch
Where Deleted = 0 -- AND InventoryBatchStatusId = 2 -- Active

-- Total credit/debit for owner's investment/drawings:
select SUM(td.Amount * td.Direction * act.CreditDebit) AS TotalBalance
from finance.TransactionDetail td
inner join finance.Account a ON td.AccountId = a.Id
inner join finance.AccountType act on a.AccountTypeId = act.Id
where td.AccountId = 3


-- Quantity of stock remaining at each location
select l.[Name] AS LocationName, pt.ProductTypeName, p.ProductName, SUM(ib.QuantityRemaining) AS ActiveQuantity, SUM(ib.QuantityRemaining) AS PendingQuantity
from finance.InventoryBatch ib
INNER JOIN Product p ON ib.ProductId = p.Id
INNER JOIN ProductType pt ON ib.ProductTypeId = pt.Id
INNER JOIN Location l ON ib.LocationId = l.Id
WHERE ib.Deleted = 0 AND ib.InventoryBatchStatusId = 2 /* active */ AND ib.QuantityRemaining > 0
GROUP BY l.[Name], pt.ProductTypeName, p.ProductName

-- Quantity of stock remaining at each location
SELECT
    l.[Name] AS LocationName,
    pt.ProductTypeName,
    p.ProductName,
    SUM(CASE WHEN ib.InventoryBatchStatusId = 2 THEN ib.QuantityRemaining ELSE 0 END) AS ActiveQuantity,
    SUM(CASE WHEN ib.InventoryBatchStatusId = 1 THEN ib.QuantityRemaining ELSE 0 END) AS PendingQuantity
FROM finance.InventoryBatch ib
INNER JOIN Product p ON ib.ProductId = p.Id
INNER JOIN ProductType pt ON ib.ProductTypeId = pt.Id
INNER JOIN Location l ON ib.LocationId = l.Id
WHERE ib.Deleted = 0 AND ib.QuantityRemaining > 0
GROUP BY l.[Name], pt.ProductTypeName, p.ProductName

-- Trial Balance Report
DECLARE @ToDate DATETIME
SET @ToDate = '2025-11-01';

SELECT 
    a.Id AS AccountId,
    a.Name AS AccountName,
    act.Type AS AccountType,
    Debit = SUM(
        CASE 
            WHEN (td.Amount * td.Direction * act.CreditDebit) > 0 
            THEN (td.Amount * td.Direction * act.CreditDebit) 
            ELSE 0 
        END
    ),
    Credit = SUM(
        CASE 
            WHEN (td.Amount * td.Direction * act.CreditDebit) < 0 
            THEN ABS(td.Amount * td.Direction * act.CreditDebit) 
            ELSE 0 
        END
    )
FROM finance.TransactionDetail td
INNER JOIN finance.Account a 
    ON td.AccountId = a.Id
INNER JOIN finance.AccountType act 
    ON a.AccountTypeId = act.Id
WHERE td.Date < @ToDate
GROUP BY a.Id, a.Name, act.Type
ORDER BY a.Name;
-- End of Trial Balance Report

-- Profit and Loss Report
DECLARE @FromDate DATETIME = '2025-01-01';
DECLARE @ToDate   DATETIME = '2025-11-01';

SELECT 
    a.Id AS AccountId,
    a.Name AS AccountName,
    act.Type AS AccountType,
    Balance = SUM(td.Amount * td.Direction * act.CreditDebit)
FROM finance.TransactionDetail td
INNER JOIN finance.Account a 
    ON td.AccountId = a.Id
INNER JOIN finance.AccountType act 
    ON a.AccountTypeId = act.Id
WHERE td.Date >= @FromDate
  AND td.Date < @ToDate
  AND act.Type IN ('Revenue','Expense')   -- only P&L accounts
GROUP BY a.Id, a.Name, act.Type
ORDER BY act.Type, a.Name;

-- End of Profit and Loss Report

-- "Full set of accounts" report. Combined Balance Sheet and Profit & Loss in a single.
DECLARE @FromDate DATETIME = '2025-01-01';
DECLARE @ToDate   DATETIME = '2025-11-01';

SELECT 
    a.Id AS AccountId,
    a.Name AS AccountName,
    act.Type AS AccountType,
    SUM(td.Amount * td.Direction * act.CreditDebit) AS Balance
FROM finance.TransactionDetail td
INNER JOIN finance.Account a 
    ON td.AccountId = a.Id
INNER JOIN finance.AccountType act 
    ON a.AccountTypeId = act.Id
WHERE (
        -- Balance Sheet accounts: Assets, Liabilities, Equity
        (act.Type IN ('Asset','Liability','Equity') AND td.Date < @ToDate)
        
        OR
        
        -- Profit & Loss accounts: Revenue & Expenses
        (act.Type IN ('Revenue','Expense') AND td.Date >= @FromDate AND td.Date < @ToDate)
      )
GROUP BY a.Id, a.Name, act.Type
ORDER BY 
    CASE act.Type 
        WHEN 'Asset' THEN 1
        WHEN 'Liability' THEN 2
        WHEN 'Equity' THEN 3
        WHEN 'Revenue' THEN 4
        WHEN 'Expense' THEN 5
    END,
    a.Name;


-- ============================================================================
-- V2.1.0 - British Summer Time date correction, and date columns to DATE
-- ============================================================================
-- Run this ONCE against the production database, BEFORE deploying the V2.1.0
-- DACPAC (the DACPAC also narrows these columns to DATE, but doing the data
-- correction first means the wrong dates are corrected rather than truncated).
--
-- BACK UP THE DATABASE FIRST.
--
-- Background
-- ----------
-- The Blazor client sent dates as local time. During British Summer Time that
-- serialised as "2026-03-31T00:00:00+01:00"; the app container runs in UTC, so
-- the value was converted back to 2026-03-30T23:00:00 and stored a day early.
-- Through the winter the offset is zero and nothing looked wrong, so only dates
-- entered during BST are affected.
--
-- The signature is exact and unambiguous: a stored time of 23:00:00. No date is
-- legitimately recorded with a time component of exactly 23:00:00 - the date
-- pickers send midnight - so this is safe to key on.
--
-- Re-running is safe. Each step checks whether it still applies: once a column
-- has been narrowed to DATE it can hold no time component, and the correction
-- and conversion steps for that column are skipped.
-- ============================================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- ---------------------------------------------------------------------------
-- Which columns still need migrating?
--
-- A column that is already DATE cannot carry a time, so it is already done.
-- Note that CAST(x AS TIME) raises an error on a DATE column rather than
-- returning midnight, so every query below has to be guarded this way.
-- ---------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#Target') IS NOT NULL DROP TABLE #Target;

CREATE TABLE #Target
(
    TableName    SYSNAME,
    ColumnName   SYSNAME,
    CurrentType  SYSNAME,
    NeedsMigrate BIT
);

INSERT INTO #Target (TableName, ColumnName, CurrentType, NeedsMigrate)
SELECT v.TableName, v.ColumnName, t.name,
       CASE WHEN t.name = 'datetime' THEN 1 ELSE 0 END
FROM (VALUES
        ('finance.[Transaction]',     'Date'),
        ('finance.TransactionDetail', 'Date'),
        ('dbo.StockSale',             'Date'),
        ('dbo.StockOrder',            'Date'),
        ('finance.InventoryBatch',    'PurchaseDate')
     ) AS v(TableName, ColumnName)
INNER JOIN sys.columns c ON c.object_id = OBJECT_ID(v.TableName) AND c.name = v.ColumnName
INNER JOIN sys.types t   ON c.user_type_id = t.user_type_id;

PRINT '=== Current column types ===';
SELECT TableName, ColumnName, CurrentType, NeedsMigrate FROM #Target ORDER BY TableName;

IF NOT EXISTS (SELECT 1 FROM #Target WHERE NeedsMigrate = 1)
BEGIN
    PRINT 'All columns are already DATE. Nothing to do.';
END
GO

-- ---------------------------------------------------------------------------
-- 1. Report what is about to change
-- ---------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM #Target WHERE NeedsMigrate = 1)
BEGIN
    PRINT '=== BEFORE: rows affected by the BST bug ===';

    DECLARE @sql NVARCHAR(MAX) = N'';

    SELECT @sql = @sql +
        CASE WHEN @sql = N'' THEN N'' ELSE N' UNION ALL ' END +
        N'SELECT ''' + TableName + N''' AS TableName, COUNT(*) AS RowsToFix FROM ' + TableName +
        N' WHERE CAST(' + QUOTENAME(ColumnName) + N' AS TIME) = ''23:00:00'''
    FROM #Target WHERE NeedsMigrate = 1;

    EXEC sp_executesql @sql;

    IF EXISTS (SELECT 1 FROM #Target
               WHERE TableName = 'finance.TransactionDetail' AND NeedsMigrate = 1)
    BEGIN
        PRINT '=== Detail of the transaction lines being moved forward one day ===';

        SELECT TOP 200
            td.Id,
            CONVERT(varchar, td.[Date], 120)                   AS StoredDate,
            CONVERT(varchar, DATEADD(HOUR, 1, td.[Date]), 120) AS CorrectedDate,
            td.Amount,
            LEFT(td.[Description], 60)                         AS [Description]
        FROM finance.TransactionDetail td
        WHERE CAST(td.[Date] AS TIME) = '23:00:00'
        ORDER BY td.[Date];
    END
END
GO

-- ---------------------------------------------------------------------------
-- 2. Correct the dates
--
-- Adding one hour moves 23:00 on the previous day to 00:00 on the correct day.
-- ---------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM #Target WHERE NeedsMigrate = 1)
BEGIN
    BEGIN TRANSACTION;

    DECLARE @table SYSNAME, @column SYSNAME, @stmt NVARCHAR(MAX), @rows INT;

    DECLARE curTargets CURSOR LOCAL FAST_FORWARD FOR
        SELECT TableName, ColumnName FROM #Target WHERE NeedsMigrate = 1;

    OPEN curTargets;
    FETCH NEXT FROM curTargets INTO @table, @column;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @stmt = N'UPDATE ' + @table +
                    N' SET ' + QUOTENAME(@column) + N' = DATEADD(HOUR, 1, ' + QUOTENAME(@column) + N')' +
                    N' WHERE CAST(' + QUOTENAME(@column) + N' AS TIME) = ''23:00:00''';

        EXEC sp_executesql @stmt;
        SET @rows = @@ROWCOUNT;
        PRINT CONCAT(@table, '.', @column, ' rows corrected: ', @rows);

        FETCH NEXT FROM curTargets INTO @table, @column;
    END

    CLOSE curTargets;
    DEALLOCATE curTargets;

    COMMIT TRANSACTION;
END
GO

-- ---------------------------------------------------------------------------
-- 3. Verify
-- ---------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM #Target WHERE NeedsMigrate = 1)
BEGIN
    PRINT '=== AFTER: rows still showing the BST signature (all must be zero) ===';

    DECLARE @check NVARCHAR(MAX) = N'';

    SELECT @check = @check +
        CASE WHEN @check = N'' THEN N'' ELSE N' UNION ALL ' END +
        N'SELECT ''' + TableName + N''' AS TableName, COUNT(*) AS RemainingRows FROM ' + TableName +
        N' WHERE CAST(' + QUOTENAME(ColumnName) + N' AS TIME) = ''23:00:00'''
    FROM #Target WHERE NeedsMigrate = 1;

    EXEC sp_executesql @check;

    PRINT '=== Times that will be discarded by the conversion (review before proceeding) ===';

    DECLARE @times NVARCHAR(MAX) = N'';

    SELECT @times = @times +
        CASE WHEN @times = N'' THEN N'' ELSE N' UNION ALL ' END +
        N'SELECT ''' + TableName + N''' AS TableName, COUNT(*) AS RowsWithATime FROM ' + TableName +
        N' WHERE CAST(' + QUOTENAME(ColumnName) + N' AS TIME) <> ''00:00:00'''
    FROM #Target WHERE NeedsMigrate = 1;

    EXEC sp_executesql @times;
END

PRINT '=== Unbalanced transactions (must return no rows) ===';

SELECT t.Id, t.Reference, SUM(td.Amount * td.Direction) AS NetDrCr
FROM finance.[Transaction] t
INNER JOIN finance.TransactionDetail td ON t.Id = td.TransactionId AND td.Deleted = 0
WHERE t.Deleted = 0
GROUP BY t.Id, t.Reference
HAVING SUM(td.Amount * td.Direction) <> 0;

PRINT '=== A transaction header and its lines must share the same date (must return no rows) ===';

SELECT t.Id, CONVERT(varchar, t.[Date], 120) AS HeaderDate,
       CONVERT(varchar, td.[Date], 120) AS LineDate
FROM finance.[Transaction] t
INNER JOIN finance.TransactionDetail td ON t.Id = td.TransactionId
WHERE t.Deleted = 0 AND td.Deleted = 0
  AND CAST(t.[Date] AS DATE) <> CAST(td.[Date] AS DATE);
GO

-- ---------------------------------------------------------------------------
-- 4. Narrow the columns to DATE
--
-- These hold calendar dates, not instants. Storing them as DATE means no time
-- component can ever be introduced again, whatever a client sends. CreateDate
-- and AmendDate stay as DATETIME - those are genuine audit timestamps.
--
-- Any remaining time components are discarded here, which is why step 2 must
-- run first. Proceed only if steps 1-3 reported clean results.
--
-- The DACPAC deploy performs the same change; running it here first keeps the
-- deploy quick and lets the results be checked.
-- ---------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM #Target WHERE NeedsMigrate = 1)
BEGIN
    DECLARE @alterTable SYSNAME, @alterColumn SYSNAME, @alter NVARCHAR(MAX);

    DECLARE curAlter CURSOR LOCAL FAST_FORWARD FOR
        SELECT TableName, ColumnName FROM #Target WHERE NeedsMigrate = 1;

    OPEN curAlter;
    FETCH NEXT FROM curAlter INTO @alterTable, @alterColumn;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @alter = N'ALTER TABLE ' + @alterTable +
                     N' ALTER COLUMN ' + QUOTENAME(@alterColumn) + N' DATE NOT NULL';
        PRINT @alter;
        EXEC sp_executesql @alter;

        FETCH NEXT FROM curAlter INTO @alterTable, @alterColumn;
    END

    CLOSE curAlter;
    DEALLOCATE curAlter;
END
GO

PRINT '=== Final column types (all must be date) ===';

SELECT OBJECT_SCHEMA_NAME(c.object_id) + '.' + OBJECT_NAME(c.object_id) AS TableName,
       c.name AS ColumnName, t.name AS DataType
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE (c.object_id = OBJECT_ID('finance.Transaction')       AND c.name = 'Date')
   OR (c.object_id = OBJECT_ID('finance.TransactionDetail') AND c.name = 'Date')
   OR (c.object_id = OBJECT_ID('dbo.StockSale')             AND c.name = 'Date')
   OR (c.object_id = OBJECT_ID('dbo.StockOrder')            AND c.name = 'Date')
   OR (c.object_id = OBJECT_ID('finance.InventoryBatch')    AND c.name = 'PurchaseDate')
ORDER BY TableName;

DROP TABLE #Target;
GO

PRINT 'V2.1.0 date migration complete.';
GO

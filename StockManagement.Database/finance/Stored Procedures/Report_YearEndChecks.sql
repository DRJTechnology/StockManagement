-- =============================================================================
-- Author:      Dave Brown
-- Create date: 02 Aug 2026
-- Description: Year-end cut-off checks. Run before closing the accounts.
--
--              Cost of goods sold is posted at the sale date but the matching
--              revenue is posted at the payment date, so a sale made near the
--              year end can have its cost in one year and its income in the
--              next. This lists anything straddling the boundary, plus sales
--              and stock orders left in an incomplete state at the year end,
--              so the accountant can journal for them.
--
--              There is deliberately no check here for the British Summer Time
--              date corruption fixed in V2.1.0. The date columns are now DATE,
--              so they cannot carry the 23:00 time that was the signature of
--              that bug - the condition is unreachable, and CAST(... AS TIME)
--              on a DATE column will not even compile. The V2.1.0 migration
--              script reports on and corrects the affected rows.
-- =============================================================================
CREATE PROCEDURE [finance].[Report_YearEndChecks]
    @FromDate   DATE,
    @ToDate     DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

    ;WITH SaleDates AS (
        SELECT ss.Id AS StockSaleId, ss.TotalPrice, c.[Name] AS ContactName,
               ss.SaleConfirmed, ss.PaymentReceived,
               CAST(MIN(cogs.[Date]) AS DATE) AS CogsDate,
               CAST(MIN(rev.[Date])  AS DATE) AS RevenueDate
        FROM dbo.StockSale ss
        INNER JOIN dbo.Contact c ON ss.ContactId = c.Id
        LEFT JOIN dbo.StockSaleDetail ssd       ON ssd.StockSaleId = ss.Id AND ssd.Deleted = 0
        LEFT JOIN finance.TransactionDetail cogs ON cogs.Id = ssd.TransactionDetailId AND cogs.Deleted = 0
        LEFT JOIN finance.TransactionDetail rev  ON rev.TransactionId = ss.TransactionId
                                                 AND rev.Deleted = 0
                                                 AND rev.AccountId IN (SELECT Id FROM finance.Account WHERE AccountTypeId = 3)
        WHERE ss.Deleted = 0
        GROUP BY ss.Id, ss.TotalPrice, c.[Name], ss.SaleConfirmed, ss.PaymentReceived
    )
    SELECT CheckType, Severity, Details, Date1, Date2, Amount
    FROM (
        -- Cost in this year, income in the next
        SELECT 'Cost in period, income after period end' AS CheckType,
               'Review' AS Severity,
               'Sale ' + CAST(StockSaleId AS NVARCHAR(20)) + ' - ' + ContactName AS Details,
               CogsDate AS Date1, RevenueDate AS Date2, TotalPrice AS Amount
        FROM SaleDates
        WHERE CogsDate >= @FromDate AND CogsDate < @ToExclusive AND RevenueDate >= @ToExclusive

        UNION ALL
        -- Income in this year, cost in the next
        SELECT 'Income in period, cost after period end', 'Review',
               'Sale ' + CAST(StockSaleId AS NVARCHAR(20)) + ' - ' + ContactName,
               RevenueDate, CogsDate, TotalPrice
        FROM SaleDates
        WHERE RevenueDate >= @FromDate AND RevenueDate < @ToExclusive AND CogsDate >= @ToExclusive

        UNION ALL
        -- Stock gone, never invoiced/paid
        SELECT 'Sale confirmed but payment never received', 'Action',
               'Sale ' + CAST(StockSaleId AS NVARCHAR(20)) + ' - ' + ContactName,
               CogsDate, NULL, TotalPrice
        FROM SaleDates
        WHERE SaleConfirmed = 1 AND PaymentReceived = 0
          AND CogsDate < @ToExclusive

        UNION ALL
        -- Paid for stock not yet received at the year end
        SELECT 'Stock paid for but not received at period end', 'Review',
               'Order ' + CAST(so.Id AS NVARCHAR(20)) + ' - ' + c.[Name],
               CAST(so.[Date] AS DATE), NULL, so.TotalCost
        FROM dbo.StockOrder so
        INNER JOIN dbo.Contact c ON so.ContactId = c.Id
        WHERE so.Deleted = 0 AND so.PaymentRecorded = 1 AND so.StockReceiptRecorded = 0
          AND so.[Date] < @ToExclusive
    ) x
    ORDER BY Severity, CheckType, Date1;
END

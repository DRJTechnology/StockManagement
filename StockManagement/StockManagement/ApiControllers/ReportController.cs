using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Models.Dto.Finance;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(ILogger<ReportController> logger, IReportService reportService) : ControllerBase
    {
        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport(int salesReportType, int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var reportItems = await reportService.GetSalesReportAsync(salesReportType, locationId, customerId, productTypeId, productId, fromDate, toDate);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetSalesReport");
                return this.BadRequest();
            }
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetStockReport(int locationId, int productTypeId, int productId)
        {
            try
            {
                var reportItems = await reportService.GetStockReportAsync(locationId, productTypeId, productId);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetStockReport");
                return this.BadRequest();
            }
        }

        [HttpGet("balancesheet")]
        public async Task<IActionResult> GetBalanceSheetReport(DateTime fromDate, DateTime toDate, AccountingBasis basis = AccountingBasis.Accruals)
        {
            try
            {
                var reportItems = await reportService.GetBalanceSheetReportAsync(fromDate, toDate, basis);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetBalanceSheetReport");
                return this.BadRequest();
            }
        }

        [HttpGet("trialbalance")]
        public async Task<IActionResult> GetTrialBalanceReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var reportItems = await reportService.GetTrialBalanceReportAsync(fromDate, toDate);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetTrialBalanceReport");
                return this.BadRequest();
            }
        }

        [HttpGet("profitandloss")]
        public async Task<IActionResult> GetProfitAndLossReport(DateTime fromDate, DateTime toDate, AccountingBasis basis = AccountingBasis.Accruals)
        {
            try
            {
                var reportItems = await reportService.GetProfitAndLossReportAsync(fromDate, toDate, basis);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetProfitAndLossReport");
                return this.BadRequest();
            }
        }

        [HttpGet("inventoryvalue")]
        public async Task<IActionResult> GetInventoryValueReport()
        {
            try
            {
                var totalValue = await reportService.GetInventoryValueReportAsync();
                return this.Ok(totalValue);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetInventoryValueReport");
                return this.BadRequest();
            }
        }

        [HttpGet("stockvaluation")]
        public async Task<IActionResult> GetStockValuationReport(DateTime asAtDate, int locationId, int productTypeId, int productId)
        {
            try
            {
                var reportItems = await reportService.GetStockValuationReportAsync(asAtDate, locationId, productTypeId, productId);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetStockValuationReport");
                return this.BadRequest();
            }
        }

        [HttpGet("stockreconciliation")]
        public async Task<IActionResult> GetStockReconciliationReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var reportItems = await reportService.GetStockReconciliationReportAsync(fromDate, toDate);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetStockReconciliationReport");
                return this.BadRequest();
            }
        }

        [HttpGet("nominalledger")]
        public async Task<IActionResult> GetNominalLedgerReport(DateTime fromDate, DateTime toDate, int accountId)
        {
            try
            {
                var reportItems = await reportService.GetNominalLedgerReportAsync(fromDate, toDate, accountId);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetNominalLedgerReport");
                return this.BadRequest();
            }
        }

        [HttpGet("incomeexpenditure")]
        public async Task<IActionResult> GetIncomeExpenditureReport(DateTime fromDate, DateTime toDate, int sectionId)
        {
            try
            {
                var reportItems = await reportService.GetIncomeExpenditureReportAsync(fromDate, toDate, sectionId);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetIncomeExpenditureReport");
                return this.BadRequest();
            }
        }

        [HttpGet("ownersaccount")]
        public async Task<IActionResult> GetOwnersAccountReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var reportItems = await reportService.GetOwnersAccountReportAsync(fromDate, toDate);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetOwnersAccountReport");
                return this.BadRequest();
            }
        }

        [HttpGet("yearendchecks")]
        public async Task<IActionResult> GetYearEndChecksReport(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var reportItems = await reportService.GetYearEndChecksReportAsync(fromDate, toDate);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ReportController)}: GetYearEndChecksReport");
                return this.BadRequest();
            }
        }
    }
}

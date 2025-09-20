using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(ILogger<ReportController> logger, IReportService reportService) : ControllerBase
    {
        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport(int locationId, int productTypeId, int productId)
        {
            try
            {
                var reportItems = await reportService.GetSalesReportAsync(locationId, productTypeId, productId);
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetSalesReport");
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
                logger.LogError(ex, "GetStockReport");
                return this.BadRequest();
            }
        }

        [HttpGet("balancesheet")]
        public async Task<IActionResult> GetBalanceSheetReport()
        {
            try
            {
                var reportItems = await reportService.GetBalanceSheetReportAsync();
                return this.Ok(reportItems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetBalanceSheetReport");
                return this.BadRequest();
            }
        }
    }
}

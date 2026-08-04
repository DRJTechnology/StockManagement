using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using StockManagement.PdfDocuments;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController(ILogger<StockSaleController> logger, IDeliveryNoteService deliveryNoteService, IStockSaleService stockSaleService, ISettingService settingService, IReportService reportService) : ControllerBase
    {
        [HttpGet("invoice/{id}")]
        public async Task<IActionResult> GetStockSalePdf(int id)
        {
            try
            {
                var stockSale = await stockSaleService.GetByIdAsync(id);
                if (stockSale == null)
                {
                    return NotFound();
                }
                byte[] logoImage = System.IO.File.ReadAllBytes("wwwroot/images/logo.jpg");
                var document = new InvoiceDocument(stockSale, logoImage, await settingService.GetAllAsync());
                var pdfBytes = document.GeneratePdf();
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(PdfController)}: GetStockSalePdf");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the PDF.");
            }
        }
        [HttpGet("delivery-note/{deliveryNoteId}")]
        public async Task<IActionResult> GetDeliveryNotePdf(int deliveryNoteId)
        {
            try
            {
                var deliveryNote = await deliveryNoteService.GetByIdAsync(deliveryNoteId);
                if (deliveryNote == null)
                {
                    return NotFound();
                }
                byte[] logoImage = System.IO.File.ReadAllBytes("wwwroot/images/logo.jpg");
                var document = new DeliveryNoteDocument(deliveryNote, logoImage, await settingService.GetAllAsync());
                var pdfBytes = document.GeneratePdf();
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(PdfController)}: GetDeliveryNotePdf");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the PDF.");
            }
        }

        // ---- Year-end finance reports -----------------------------------------

        [HttpGet("profit-and-loss")]
        public Task<IActionResult> GetProfitAndLossPdf(DateTime fromDate, DateTime toDate, AccountingBasis basis = AccountingBasis.Accruals)
            => GeneratePdfAsync(nameof(GetProfitAndLossPdf), $"Profit and Loss {FileSuffix(fromDate, toDate)}",
                async (logo, settings) => new ProfitAndLossDocument(
                    await reportService.GetProfitAndLossReportAsync(fromDate, toDate, basis),
                    fromDate, toDate, basis, logo, settings));

        [HttpGet("balance-sheet")]
        public Task<IActionResult> GetBalanceSheetPdf(DateTime fromDate, DateTime toDate, AccountingBasis basis = AccountingBasis.Accruals)
            => GeneratePdfAsync(nameof(GetBalanceSheetPdf), $"Balance Sheet at {toDate:yyyy-MM-dd}",
                async (logo, settings) => new BalanceSheetDocument(
                    await reportService.GetBalanceSheetReportAsync(fromDate, toDate, basis),
                    toDate, basis, logo, settings));

        [HttpGet("trial-balance")]
        public Task<IActionResult> GetTrialBalancePdf(DateTime fromDate, DateTime toDate)
            => GeneratePdfAsync(nameof(GetTrialBalancePdf), $"Trial Balance {FileSuffix(fromDate, toDate)}",
                async (logo, settings) => new TrialBalanceDocument(
                    await reportService.GetTrialBalanceReportAsync(fromDate, toDate),
                    fromDate, toDate, logo, settings));

        [HttpGet("stock-valuation")]
        public Task<IActionResult> GetStockValuationPdf(DateTime asAtDate)
            => GeneratePdfAsync(nameof(GetStockValuationPdf), $"Stock Valuation at {asAtDate:yyyy-MM-dd}",
                async (logo, settings) => new StockValuationDocument(
                    await reportService.GetStockValuationReportAsync(asAtDate, 0, 0, 0),
                    asAtDate, logo, settings));

        [HttpGet("stock-reconciliation")]
        public Task<IActionResult> GetStockReconciliationPdf(DateTime fromDate, DateTime toDate)
            => GeneratePdfAsync(nameof(GetStockReconciliationPdf), $"Stock Reconciliation {FileSuffix(fromDate, toDate)}",
                async (logo, settings) => new StockReconciliationDocument(
                    await reportService.GetStockReconciliationReportAsync(fromDate, toDate),
                    fromDate, toDate, logo, settings));

        [HttpGet("nominal-ledger")]
        public Task<IActionResult> GetNominalLedgerPdf(DateTime fromDate, DateTime toDate)
            => GeneratePdfAsync(nameof(GetNominalLedgerPdf), $"Nominal Ledger {FileSuffix(fromDate, toDate)}",
                async (logo, settings) => new NominalLedgerDocument(
                    await reportService.GetNominalLedgerReportAsync(fromDate, toDate, 0),
                    fromDate, toDate, logo, settings));

        [HttpGet("income-expenditure")]
        public Task<IActionResult> GetIncomeExpenditurePdf(DateTime fromDate, DateTime toDate)
            => GeneratePdfAsync(nameof(GetIncomeExpenditurePdf), $"Income and Expenditure {FileSuffix(fromDate, toDate)}",
                async (logo, settings) => new IncomeExpenditureDocument(
                    await reportService.GetIncomeExpenditureReportAsync(fromDate, toDate, 0),
                    fromDate, toDate, logo, settings));

        [HttpGet("owners-account")]
        public Task<IActionResult> GetOwnersAccountPdf(DateTime fromDate, DateTime toDate)
            => GeneratePdfAsync(nameof(GetOwnersAccountPdf), $"Owners Account {FileSuffix(fromDate, toDate)}",
                async (logo, settings) => new OwnersAccountDocument(
                    await reportService.GetOwnersAccountReportAsync(fromDate, toDate),
                    fromDate, toDate, logo, settings));

        [HttpGet("year-end-checks")]
        public Task<IActionResult> GetYearEndChecksPdf(DateTime fromDate, DateTime toDate)
            => GeneratePdfAsync(nameof(GetYearEndChecksPdf), $"Year End Checks {FileSuffix(fromDate, toDate)}",
                async (logo, settings) => new YearEndChecksDocument(
                    await reportService.GetYearEndChecksReportAsync(fromDate, toDate),
                    fromDate, toDate, logo, settings));

        /// <summary>
        /// Every year-end report merged into a single PDF, in the order an
        /// accountant reads them. This is the file to send them.
        /// </summary>
        [HttpGet("year-end-pack")]
        public async Task<IActionResult> GetYearEndPackPdf(DateTime fromDate, DateTime toDate, AccountingBasis basis = AccountingBasis.Accruals)
        {
            try
            {
                var logo = await LoadLogoAsync();
                var settings = await settingService.GetAllAsync();

                var documents = new IDocument[]
                {
                    new ProfitAndLossDocument(await reportService.GetProfitAndLossReportAsync(fromDate, toDate, basis), fromDate, toDate, basis, logo, settings),
                    new BalanceSheetDocument(await reportService.GetBalanceSheetReportAsync(fromDate, toDate, basis), toDate, basis, logo, settings),
                    new TrialBalanceDocument(await reportService.GetTrialBalanceReportAsync(fromDate, toDate), fromDate, toDate, logo, settings),
                    new StockValuationDocument(await reportService.GetStockValuationReportAsync(toDate, 0, 0, 0), toDate, logo, settings),
                    new StockReconciliationDocument(await reportService.GetStockReconciliationReportAsync(fromDate, toDate), fromDate, toDate, logo, settings),
                    new IncomeExpenditureDocument(await reportService.GetIncomeExpenditureReportAsync(fromDate, toDate, 0), fromDate, toDate, logo, settings),
                    new OwnersAccountDocument(await reportService.GetOwnersAccountReportAsync(fromDate, toDate), fromDate, toDate, logo, settings),
                    new NominalLedgerDocument(await reportService.GetNominalLedgerReportAsync(fromDate, toDate, 0), fromDate, toDate, logo, settings),
                    new YearEndChecksDocument(await reportService.GetYearEndChecksReportAsync(fromDate, toDate), fromDate, toDate, logo, settings),
                };

                var pdfBytes = Document.Merge(documents).GeneratePdf();
                return File(pdfBytes, "application/pdf", $"Accounts {FileSuffix(fromDate, toDate)}.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(PdfController)}: {nameof(GetYearEndPackPdf)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the PDF.");
            }
        }

        private static string FileSuffix(DateTime fromDate, DateTime toDate)
            => $"{fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}";

        private static Task<byte[]> LoadLogoAsync()
            => System.IO.File.ReadAllBytesAsync("wwwroot/images/logo.jpg");

        private async Task<IActionResult> GeneratePdfAsync(
            string action, string fileName, Func<byte[], List<SettingResponseModel>, Task<IDocument>> build)
        {
            try
            {
                var logo = await LoadLogoAsync();
                var settings = await settingService.GetAllAsync();
                var document = await build(logo, settings);
                var pdfBytes = document.GeneratePdf();
                return File(pdfBytes, "application/pdf", $"{fileName}.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(PdfController)}: {action}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the PDF.");
            }
        }
    }
}

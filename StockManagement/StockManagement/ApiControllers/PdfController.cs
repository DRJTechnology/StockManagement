using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController(ILogger<StockSaleController> logger, IStockSaleService stockSaleService, ISettingService settingService) : ControllerBase
    {
        [HttpGet("stock-sale/{id}")]
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
                //var logoImage = await stockSaleService.GetLogoImageAsync(); // Assuming this method exists
                var document = new StockSaleDocument(stockSale, logoImage, await settingService.GetAllAsync());
                var pdfBytes = document.GeneratePdf(); // Assuming GeneratePdf is a method that creates the PDF
                return File(pdfBytes, "application/pdf");
                //return File(pdfBytes, "application/pdf", $"StockSale_{id}.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating delivery note PDF");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the PDF.");
            }
        }
    }
}

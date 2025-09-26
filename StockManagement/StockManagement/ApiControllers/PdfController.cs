using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController(ILogger<StockSaleController> logger, IDeliveryNoteService deliveryNoteService, IStockSaleService stockSaleService, ISettingService settingService) : ControllerBase
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
    }
}

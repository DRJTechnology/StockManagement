using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using StockManagement.Components.Account;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController(ILogger<DeliveryNoteController> logger, IDeliveryNoteService deliveryNoteService) : ControllerBase
    {
        [HttpGet("delivery-note/{id}")]
        public async Task<IActionResult> GetDeliveryNotePdf(int id)
        {
            try
            {
                var deliveryNote = await deliveryNoteService.GetByIdAsync(id);
                if (deliveryNote == null)
                {
                    return NotFound();
                }
                byte[] logoImage = System.IO.File.ReadAllBytes("wwwroot/images/logo.jpg");
                //var logoImage = await deliveryNoteService.GetLogoImageAsync(); // Assuming this method exists
                var document = new DeliveryNoteDocument(deliveryNote, logoImage);
                var pdfBytes = document.GeneratePdf(); // Assuming GeneratePdf is a method that creates the PDF
                return File(pdfBytes, "application/pdf", $"DeliveryNote_{id}.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating delivery note PDF");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the PDF.");
            }
        }
    }
}

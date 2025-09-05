using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Models;
using StockManagement.Services.Interfaces;

namespace StockManagement.ApiControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryBatchController(ILogger<InventoryBatchController> logger, IInventoryBatchService inventoryBatchService) : ControllerBase
    {
        [HttpGet("GetFiltered")]
        public async Task<IActionResult> GetFiltered([FromQuery] InventoryBatchFilterModel inventoryBatchFilterModel)
        {
            try
            {
                var filteredInventory = await inventoryBatchService.GetFilteredAsync(inventoryBatchFilterModel);
                return this.Ok(filteredInventory);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetFiltered");
                return this.BadRequest();
            }
        }

        [HttpGet("GetActivity/{inventoryBatchId}")]
        public async Task<IActionResult> GetActivity(int inventoryBatchId)
        {
            try
            {
                var inventoryBatchActivity = await inventoryBatchService.GetActivityAsync(inventoryBatchId);
                return this.Ok(inventoryBatchActivity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetActivity");
                return this.BadRequest();
            }
        }
    }
}

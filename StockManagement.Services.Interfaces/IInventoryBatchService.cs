using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IInventoryBatchService
    {
        Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel);
    }
}

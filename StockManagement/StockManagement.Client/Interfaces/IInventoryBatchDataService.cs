using StockManagement.Models;

namespace StockManagement.Client.Interfaces
{
    public interface IInventoryBatchDataService
    {
        Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel);
    }
}

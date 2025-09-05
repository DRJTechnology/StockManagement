using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.Client.Interfaces
{
    public interface IInventoryBatchDataService
    {
        Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel);
        Task<List<InventoryBatchActivityDto>> GetActivityAsync(int inventoryBatchId);
    }
}

using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.Repositories.Interfaces
{
    public interface IInventoryBatchRepository
    {
        Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel);
        Task<List<InventoryBatchActivityDto>> GetActivityAsync(int inventoryBatchId);
    }
}

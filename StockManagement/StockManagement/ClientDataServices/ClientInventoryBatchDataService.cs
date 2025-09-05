using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.ClientDataServices
{
    public class ClientInventoryBatchDataService : IInventoryBatchDataService
    {
        public Task<List<InventoryBatchActivityDto>> GetActivityAsync(int inventoryBatchId)
        {
            throw new NotImplementedException();
        }

        public Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel)
        {
            throw new NotImplementedException();
        }
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientInventoryBatchDataService : IInventoryBatchDataService
    {
        public Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel)
        {
            throw new NotImplementedException();
        }
    }
}

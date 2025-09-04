using AutoMapper;
using StockManagement.Models;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class InventoryBatchService(IMapper mapper, IInventoryBatchRepository inventoryBatchRepository) : IInventoryBatchService
    {
        public async Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel)
        {
            var filteredInventory = mapper.Map<InventoryBatchFilteredResponseModel>(await inventoryBatchRepository.GetFilteredAsync(inventoryBatchFilterModel));
            return filteredInventory;
        }
    }
}

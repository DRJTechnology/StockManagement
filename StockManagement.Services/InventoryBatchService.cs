using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class InventoryBatchService(IMapper mapper, IInventoryBatchRepository inventoryBatchRepository) : IInventoryBatchService
    {
        public async Task<List<InventoryBatchActivityDto>> GetActivityAsync(int inventoryBatchId)
        {
            var inventoryBatchActivity = await inventoryBatchRepository.GetActivityAsync(inventoryBatchId);
            return inventoryBatchActivity;
        }

        public async Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel)
        {
            var filteredInventory = mapper.Map<InventoryBatchFilteredResponseModel>(await inventoryBatchRepository.GetFilteredAsync(inventoryBatchFilterModel));
            return filteredInventory;
        }
    }
}

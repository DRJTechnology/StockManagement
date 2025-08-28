using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.Services
{
    public class StockOrderService(IMapper mapper, IStockOrderRepository stockOrderRepository, ITransactionService transactionService, 
                                    IInventoryBatchRepository inventoryBatchRepository, IActivityRepository activityRepository) : IStockOrderService
    {
        public async Task<int> CreateAsync(int currentUserId, StockOrderEditModel StockOrder)
        {
            var dto = mapper.Map<StockOrderDto>(StockOrder);
            return await stockOrderRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> CreateStockOrderPayments(int currentUserId, StockOrderPaymentsCreateModel stockOrderDetailPayments)
        {
            return await stockOrderRepository.CreateStockOrderPayments(currentUserId, stockOrderDetailPayments);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockOrderId)
        {
            return await stockOrderRepository.DeleteAsync(currentUserId, StockOrderId);
        }

        public async Task<List<StockOrderResponseModel>> GetAllAsync()
        {
            var StockOrders = await stockOrderRepository.GetAllAsync();
            return mapper.Map<List<StockOrderResponseModel>>(StockOrders);
        }

        public async Task<StockOrderResponseModel> GetByIdAsync(int StockOrderId)
        {
            var StockOrder = await stockOrderRepository.GetByIdAsync(StockOrderId);
            return mapper.Map<StockOrderResponseModel>(StockOrder);
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockOrderEditModel StockOrder)
        {
            var dto = mapper.Map<StockOrderDto>(StockOrder);
            return await stockOrderRepository.UpdateAsync(currentUserId, dto);
        }
    }
}

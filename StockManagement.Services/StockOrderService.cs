using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockOrderService(IMapper mapper, IStockOrderRepository StockOrderRepository) : IStockOrderService
    {
        public async Task<int> CreateAsync(int currentUserId, StockOrderEditModel StockOrder)
        {
            var dto = mapper.Map<StockOrderDto>(StockOrder);
            return await StockOrderRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockOrderId)
        {
            return await StockOrderRepository.DeleteAsync(currentUserId, StockOrderId);
        }

        public async Task<List<StockOrderResponseModel>> GetAllAsync()
        {
            var StockOrders = await StockOrderRepository.GetAllAsync();
            return mapper.Map<List<StockOrderResponseModel>>(StockOrders);
        }

        public async Task<StockOrderResponseModel> GetByIdAsync(int StockOrderId)
        {
            var StockOrder = await StockOrderRepository.GetByIdAsync(StockOrderId);
            return mapper.Map<StockOrderResponseModel>(StockOrder);
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockOrderEditModel StockOrder)
        {
            var dto = mapper.Map<StockOrderDto>(StockOrder);
            return await StockOrderRepository.UpdateAsync(currentUserId, dto);
        }
    }
}

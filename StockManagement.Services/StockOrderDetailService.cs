using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockOrderDetailService(IMapper mapper, IStockOrderDetailRepository StockOrderDetailRepository) : IStockOrderDetailService
    {
        public async Task<int> CreateAsync(int currentUserId, StockOrderDetailEditModel StockOrder)
        {
            var dto = mapper.Map<StockOrderDetailDto>(StockOrder);
            return await StockOrderDetailRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockOrderId)
        {
            return await StockOrderDetailRepository.DeleteAsync(currentUserId, StockOrderId);
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockOrderDetailEditModel StockOrder)
        {
            var dto = mapper.Map<StockOrderDetailDto>(StockOrder);
            return await StockOrderDetailRepository.UpdateAsync(currentUserId, dto);
        }
    }
}

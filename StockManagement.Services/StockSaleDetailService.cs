using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockSaleDetailService(IMapper mapper, IStockSaleDetailRepository stockSaleDetailRepository) : IStockSaleDetailService
    {
        public async Task<int> CreateAsync(int currentUserId, StockSaleDetailEditModel stockSale)
        {
            var dto = mapper.Map<StockSaleDetailDto>(stockSale);
            return await stockSaleDetailRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int stockSaleId)
        {
            return await stockSaleDetailRepository.DeleteAsync(currentUserId, stockSaleId);
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockSaleDetailEditModel stockSale)
        {
            var dto = mapper.Map<StockSaleDetailDto>(stockSale);
            return await stockSaleDetailRepository.UpdateAsync(currentUserId, dto);
        }
    }
}

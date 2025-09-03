using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class StockSaleService(IMapper mapper, IStockSaleRepository stockSaleRepository) : IStockSaleService
    {
        public async Task<int> CreateAsync(int currentUserId, StockSaleEditModel stockSale)
        {
            var dto = mapper.Map<StockSaleDto>(stockSale);
            return await stockSaleRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int stockSaleId)
        {
            return await stockSaleRepository.DeleteAsync(currentUserId, stockSaleId);
        }

        public async Task<List<StockSaleResponseModel>> GetAllAsync()
        {
            var stockSales = await stockSaleRepository.GetAllAsync();
            return mapper.Map<List<StockSaleResponseModel>>(stockSales);
        }

        public async Task<StockSaleResponseModel> GetByIdAsync(int stockSaleId)
        {
            var stockSale = await stockSaleRepository.GetByIdAsync(stockSaleId);
            return mapper.Map<StockSaleResponseModel>(stockSale);
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockSaleEditModel stockSale)
        {
            var dto = mapper.Map<StockSaleDto>(stockSale);
            return await stockSaleRepository.UpdateAsync(currentUserId, dto);
        }
    }
}

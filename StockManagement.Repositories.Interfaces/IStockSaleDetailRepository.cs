using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IStockSaleDetailRepository
    {
        Task<int> CreateAsync(int currentUserId, StockSaleDetailDto stockSaleDetailDto);
        Task<bool> DeleteAsync(int currentUserId, int stockSaleId);
        Task<bool> UpdateAsync(int currentUserId, StockSaleDetailDto stockSaleDetailDto);
    }
}

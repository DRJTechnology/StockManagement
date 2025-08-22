using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IStockOrderDetailRepository
    {
        Task<int> CreateAsync(int currentUserId, StockOrderDetailDto StockOrderDetailDto);
        Task<bool> DeleteAsync(int currentUserId, int StockOrderId);
        Task<bool> UpdateAsync(int currentUserId, StockOrderDetailDto StockOrderDetailDto);
    }
}

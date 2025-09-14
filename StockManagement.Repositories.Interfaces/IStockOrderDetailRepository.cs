using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IStockOrderDetailRepository
    {
        Task<int> CreateAsync(int currentUserId, StockOrderDetailDto stockOrderDetailDto);
        Task<bool> DeleteAsync(int currentUserId, int stockOrderId);
        Task<bool> UpdateAsync(int currentUserId, StockOrderDetailDto stockOrderDetailDto);
    }
}

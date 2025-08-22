using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IStockOrderService
    {
        Task<int> CreateAsync(int currentUserId, StockOrderEditModel StockOrder);
        Task<bool> DeleteAsync(int currentUserId, int StockOrderId);
        Task<List<StockOrderResponseModel>> GetAllAsync();
        Task<StockOrderResponseModel> GetByIdAsync(int StockOrderId);
        Task<bool> UpdateAsync(int currentUserId, StockOrderEditModel StockOrder);
    }
}

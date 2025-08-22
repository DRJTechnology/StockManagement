using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IStockOrderDetailService
    {
        Task<int> CreateAsync(int currentUserId, StockOrderDetailEditModel StockOrder);
        Task<bool> DeleteAsync(int currentUserId, int StockOrderId);
        Task<bool> UpdateAsync(int currentUserId, StockOrderDetailEditModel StockOrder);
    }
}

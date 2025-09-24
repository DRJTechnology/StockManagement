using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IStockOrderService
    {
        Task<int> CreateAsync(int currentUserId, StockOrderEditModel StockOrder);
        Task<bool> CreateStockOrderPaymentsAsync(int currentUserId, StockOrderPaymentsCreateModel stockOrderDetailPayments);
        Task<bool> DeleteAsync(int currentUserId, int StockOrderId);
        Task<List<StockOrderResponseModel>> GetAllAsync();
        Task<StockOrderResponseModel> GetByIdAsync(int StockOrderId);
        Task<bool> MarkStockReceivedAsync(int currentUserId, int stockOrderId);
        Task<bool> UpdateAsync(int currentUserId, StockOrderEditModel StockOrder);
    }
}

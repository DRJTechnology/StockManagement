using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IStockReceiptService
    {
        Task<int> CreateAsync(int currentUserId, StockReceiptEditModel StockReceipt);
        Task<bool> DeleteAsync(int currentUserId, int StockReceiptId);
        Task<List<StockReceiptResponseModel>> GetAllAsync();
        Task<StockReceiptResponseModel> GetByIdAsync(int StockReceiptId);
        Task<bool> UpdateAsync(int currentUserId, StockReceiptEditModel StockReceipt);
    }
}

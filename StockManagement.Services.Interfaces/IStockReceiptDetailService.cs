using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IStockReceiptDetailService
    {
        Task<int> CreateAsync(int currentUserId, StockReceiptDetailEditModel StockReceipt);
        Task<bool> DeleteAsync(int currentUserId, int StockReceiptId);
        Task<bool> UpdateAsync(int currentUserId, StockReceiptDetailEditModel StockReceipt);
    }
}

using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IStockReceiptDetailRepository
    {
        Task<int> CreateAsync(int currentUserId, StockReceiptDetailDto StockReceiptDetailDto);
        Task<bool> DeleteAsync(int currentUserId, int StockReceiptId);
        Task<bool> UpdateAsync(int currentUserId, StockReceiptDetailDto StockReceiptDetailDto);
    }
}

using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IStockReceiptRepository
    {
        Task<int> CreateAsync(int currentUserId, StockReceiptDto StockReceiptDto);
        Task<bool> DeleteAsync(int currentUserId, int StockReceiptId);
        Task<bool> UpdateAsync(int currentUserId, StockReceiptDto StockReceiptDto);
        Task<List<StockReceiptDto>> GetAllAsync();
        Task<StockReceiptDto> GetByIdAsync(int StockReceiptId);
    }
}

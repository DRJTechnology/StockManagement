using StockManagement.Models;
using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IStockOrderRepository
    {
        Task<int> CreateAsync(int currentUserId, StockOrderDto StockOrderDto);
        Task<bool> DeleteAsync(int currentUserId, int StockOrderId);
        Task<bool> UpdateAsync(int currentUserId, StockOrderDto StockOrderDto);
        Task<List<StockOrderDto>> GetAllAsync();
        Task<StockOrderDto> GetByIdAsync(int StockOrderId);
        Task<bool> CreateStockOrderPayments(int currentUserId, StockOrderPaymentsCreateModel stockOrderDetailPayments);
        Task<bool> MarkStockReceivedAsync(int currentUserId, int stockOrderId);
    }
}

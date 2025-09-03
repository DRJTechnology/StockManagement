using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IStockSaleDetailService
    {
        Task<int> CreateAsync(int currentUserId, StockSaleDetailEditModel stockSale);
        Task<bool> DeleteAsync(int currentUserId, int stockSaleId);
        Task<bool> UpdateAsync(int currentUserId, StockSaleDetailEditModel stockSale);
    }
}

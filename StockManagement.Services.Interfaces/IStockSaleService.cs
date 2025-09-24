using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IStockSaleService
    {
        Task<bool> ConfirmStockSaleAsync(int currentUserId, StockSaleConfirmationModel stockSaleConfirmation);
        Task<bool> ConfirmStockSalePaymentAsync(int currentUserId, StockSaleConfirmPaymentModel stockSaleConfirmPaymentModel);
        Task<int> CreateAsync(int currentUserId, StockSaleEditModel stockSale);
        Task<bool> DeleteAsync(int currentUserId, int stockSaleId);
        Task<List<StockSaleResponseModel>> GetAllAsync();
        Task<StockSaleResponseModel> GetByIdAsync(int stockSaleId);
        Task<bool> ResetStockSaleAsync(int currentUserId, int stockSaleId);
        Task<bool> UpdateAsync(int currentUserId, StockSaleEditModel stockSale);
    }
}

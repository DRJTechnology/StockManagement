using StockManagement.Models;
using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IStockSaleRepository
    {
        Task<int> CreateAsync(int currentUserId, StockSaleDto stockSaleDto);
        Task<bool> DeleteAsync(int currentUserId, int stockSaleId);
        Task<bool> UpdateAsync(int currentUserId, StockSaleDto stockSaleDto);
        Task<List<StockSaleDto>> GetAllAsync();
        Task<StockSaleDto> GetByIdAsync(int stockSaleId);
        Task<bool> ConfirmStockSaleAsync(int currentUserId, StockSaleConfirmationModel stockSaleConfirmation);
        Task<bool> ConfirmStockSalePaymentAsync(int currentUserId, StockSaleConfirmPaymentModel stockSaleConfirmPaymentModel);
        Task<bool> ResetStockSaleAsync(int currentUserId, int stockSaleId);
    }
}

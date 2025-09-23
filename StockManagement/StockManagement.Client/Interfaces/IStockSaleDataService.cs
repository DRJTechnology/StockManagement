using StockManagement.Models;

namespace StockManagement.Client.Interfaces
{
    public interface IStockSaleDataService : IGenericDataService<StockSaleEditModel, StockSaleResponseModel>
    {
        Task<bool> ConfirmStockSale(StockSaleConfirmationModel stockSaleConfirmation);
        Task<bool> ConfirmStockSalePayment(StockSaleConfirmPaymentModel stockSaleConfirmPaymentModel);
        Task<bool> ResetAsync(int stockSaleId);
    }
}

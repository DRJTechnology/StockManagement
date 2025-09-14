using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientStockSaleDataService : IStockSaleDataService
    {
        public Task<bool> ConfirmStockSale(StockSaleConfirmationModel stockSaleConfirmation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmStockSalePayment(StockSaleConfirmPaymentModel stockSaleConfirmPaymentModel)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(StockSaleEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockSaleResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StockSaleResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StockSaleEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

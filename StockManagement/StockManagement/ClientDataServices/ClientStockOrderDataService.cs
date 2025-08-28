using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientStockOrderDataService : IStockOrderDataService
    {
        public Task<int> CreateAsync(StockOrderEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateStockOrderPayments(StockOrderPaymentsCreateModel stockOrderDetailPayments)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockOrderResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StockOrderResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StockOrderEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

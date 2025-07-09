using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientStockReceiptDataService : IStockReceiptDataService
    {
        public Task<int> CreateAsync(StockReceiptEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockReceiptResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StockReceiptResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StockReceiptEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientStockReceiptDetailDataService : IStockReceiptDetailDataService
    {
        public Task<int> CreateAsync(StockReceiptDetailEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockReceiptDetailResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StockReceiptDetailResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StockReceiptDetailEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

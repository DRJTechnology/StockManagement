using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientStockOrderDetailDataService : IStockOrderDetailDataService
    {
        public Task<int> CreateAsync(StockOrderDetailEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockOrderDetailResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StockOrderDetailResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StockOrderDetailEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

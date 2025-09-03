using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientStockSaleDetailDataService : IStockSaleDetailDataService
    {
        public Task<int> CreateAsync(StockSaleDetailEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockSaleDetailResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StockSaleDetailResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StockSaleDetailEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

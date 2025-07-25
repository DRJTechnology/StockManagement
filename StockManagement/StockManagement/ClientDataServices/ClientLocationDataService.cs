using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientLocationDataService : ILocationDataService
    {
        public Task<int> CreateAsync(LocationEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocationResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LocationResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(LocationEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

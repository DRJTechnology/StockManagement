using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientLookupsDataService : ILookupsDataService
    {
        public Task<int> CreateAsync(LookupsModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LookupsModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LookupsModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(LookupsModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

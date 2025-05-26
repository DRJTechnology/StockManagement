using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientVenueDataService : IVenueDataService
    {
        public Task<int> CreateAsync(VenueEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VenueResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<VenueResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(VenueEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

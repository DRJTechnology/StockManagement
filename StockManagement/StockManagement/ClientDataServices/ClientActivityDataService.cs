using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientActivityDataService : IActivityDataService
    {
        public Task<int> CreateAsync(ActivityEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ActivityResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActivityResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<ActivityFilteredResponseModel> GetFilteredAsync(ActivityFilterModel activityFilterModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ActivityEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

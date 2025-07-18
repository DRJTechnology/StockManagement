using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientSettingDataService : ISettingDataService
    {
        public Task<int> CreateAsync(SettingEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SettingResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SettingResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(SettingEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

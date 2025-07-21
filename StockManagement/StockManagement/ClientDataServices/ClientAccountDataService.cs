using StockManagement.Client.Interfaces;
using StockManagement.Models.Finance;

namespace StockManagement.ClientDataServices
{
    public class ClientAccountDataService : IAccountDataService
    {
        public Task<int> CreateAsync(AccountEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AccountResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AccountResponseModel>> GetAllAsync(bool includeInactive)
        {
            throw new NotImplementedException();
        }

        public Task<AccountResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(AccountEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

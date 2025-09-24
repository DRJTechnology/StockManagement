using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;

namespace StockManagement.ClientDataServices.Finance
{
    public class ClientAccountTypeDataService : IAccountTypeDataService
    {
        public Task<int> CreateAsync(AccountTypeEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AccountTypeResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AccountTypeResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(AccountTypeEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

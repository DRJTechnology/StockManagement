using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientContactDataService : IContactDataService
    {
        public Task<int> CreateAsync(ContactEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ContactResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ContactResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ContactEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

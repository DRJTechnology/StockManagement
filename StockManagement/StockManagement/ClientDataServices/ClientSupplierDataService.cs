using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientSupplierDataService : ISupplierDataService
    {
        public Task<int> CreateAsync(SupplierEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SupplierResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SupplierResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(SupplierEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

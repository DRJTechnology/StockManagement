using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientProductTypeDataService : IProductTypeDataService
    {
        public Task<int> CreateAsync(ProductTypeEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductTypeResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductTypeResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ProductTypeEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

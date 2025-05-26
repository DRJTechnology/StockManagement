using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientProductDataService : IProductDataService
    {
        public Task<int> CreateAsync(ProductEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ProductEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}

using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IProductService
    {
        Task<int> CreateAsync(int currentUserId, ProductEditModel product);
        Task<bool> DeleteAsync(int currentUserId, int productId);
        Task<List<ProductResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, ProductEditModel product);
    }
}

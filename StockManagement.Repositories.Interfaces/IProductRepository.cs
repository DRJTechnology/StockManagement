using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<int> CreateAsync(int currentUserId, ProductDto productDto);
        Task<bool> DeleteAsync(int currentUserId, int productId);
        Task<bool> UpdateAsync(int currentUserId, ProductDto productDto);
        Task<List<ProductDto>> GetAllAsync();
    }
}

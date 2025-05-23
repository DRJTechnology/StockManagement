using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IProductTypeRepository
    {
        Task<int> CreateAsync(int currentUserId, ProductTypeDto productTypeDto);
        Task<bool> DeleteAsync(int currentUserId, int productTypeId);
        Task<bool> UpdateAsync(int currentUserId, ProductTypeDto productTypeDto);
        Task<List<ProductTypeDto>> GetAllAsync();
    }
}

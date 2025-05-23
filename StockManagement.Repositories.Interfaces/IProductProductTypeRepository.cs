using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IProductProductTypeRepository
    {
        Task<int> CreateAsync(int currentUserId, ProductProductTypeDto productProductTypeDto);
        Task<bool> DeleteAsync(int currentUserId, int productProductTypeId);
        Task<bool> UpdateAsync(int currentUserId, ProductProductTypeDto productProductTypeDto);
        Task<List<ProductProductTypeDto>> GetAllAsync();
    }
}

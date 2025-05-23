using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IProductTypeService
    {
        Task<int> CreateAsync(int currentUserId, ProductTypeEditModel productType);
        Task<bool> DeleteAsync(int currentUserId, int productTypeId);
        Task<List<ProductTypeResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, ProductTypeEditModel productType);
    }
}

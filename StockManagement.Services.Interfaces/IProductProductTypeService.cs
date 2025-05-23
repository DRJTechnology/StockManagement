using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IProductProductTypeService
    {
        Task<int> CreateAsync(int currentUserId, ProductProductTypeEditModel productProductType);
        Task<bool> DeleteAsync(int currentUserId, int productProductTypeId);
        Task<List<ProductProductTypeResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, ProductProductTypeEditModel productProductType);
    }
}

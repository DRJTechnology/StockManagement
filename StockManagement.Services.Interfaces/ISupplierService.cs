using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<int> CreateAsync(int currentUserId, SupplierEditModel Supplier);
        Task<bool> DeleteAsync(int currentUserId, int SupplierId);
        Task<List<SupplierResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, SupplierEditModel Supplier);
    }
}

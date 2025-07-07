using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface ISupplierRepository
    {
        Task<int> CreateAsync(int currentUserId, SupplierDto SupplierDto);
        Task<bool> DeleteAsync(int currentUserId, int SupplierId);
        Task<bool> UpdateAsync(int currentUserId, SupplierDto SupplierDto);
        Task<List<SupplierDto>> GetAllAsync();
    }
}

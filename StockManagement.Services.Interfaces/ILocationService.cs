using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface ILocationService
    {
        Task<int> CreateAsync(int currentUserId, LocationEditModel location);
        Task<bool> DeleteAsync(int currentUserId, int locationId);
        Task<List<LocationResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, LocationEditModel location);
    }
}

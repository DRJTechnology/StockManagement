using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<int> CreateAsync(int currentUserId, LocationDto locationDto);
        Task<bool> DeleteAsync(int currentUserId, int locationId);
        Task<bool> UpdateAsync(int currentUserId, LocationDto locationDto);
        Task<List<LocationDto>> GetAllAsync();
    }
}

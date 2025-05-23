using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IVenueService
    {
        Task<int> CreateAsync(int currentUserId, VenueEditModel venue);
        Task<bool> DeleteAsync(int currentUserId, int venueId);
        Task<List<VenueResponseModel>> GetAllAsync();
        Task<bool> UpdateAsync(int currentUserId, VenueEditModel venue);
    }
}

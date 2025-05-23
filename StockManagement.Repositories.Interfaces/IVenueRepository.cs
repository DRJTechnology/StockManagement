using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IVenueRepository
    {
        Task<int> CreateAsync(int currentUserId, VenueDto venueDto);
        Task<bool> DeleteAsync(int currentUserId, int venueId);
        Task<bool> UpdateAsync(int currentUserId, VenueDto venueDto);
        Task<List<VenueDto>> GetAllAsync();
    }
}

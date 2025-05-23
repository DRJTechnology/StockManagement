using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class VenueService(IMapper mapper, IVenueRepository venueRepository) : IVenueService
    {
        public async Task<int> CreateAsync(int currentUserId, VenueEditModel venue)
        {
            var venueDto = mapper.Map<VenueDto>(venue);
            return await venueRepository.CreateAsync(currentUserId, venueDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int venueId)
        {
            return await venueRepository.DeleteAsync(currentUserId, venueId);
        }

        public async Task<List<VenueResponseModel>> GetAllAsync()
        {
            var venues = mapper.Map<List<VenueResponseModel>>(await venueRepository.GetAllAsync());
            return venues;
        }

        public async Task<bool> UpdateAsync(int currentUserId, VenueEditModel venue)
        {
            var venueDto = mapper.Map<VenueDto>(venue);
            return await venueRepository.UpdateAsync(currentUserId, venueDto);
        }
    }
}

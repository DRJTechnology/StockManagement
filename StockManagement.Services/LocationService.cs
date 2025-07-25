using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class LocationService(IMapper mapper, ILocationRepository locationRepository) : ILocationService
    {
        public async Task<int> CreateAsync(int currentUserId, LocationEditModel location)
        {
            var locationDto = mapper.Map<LocationDto>(location);
            return await locationRepository.CreateAsync(currentUserId, locationDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int locationId)
        {
            return await locationRepository.DeleteAsync(currentUserId, locationId);
        }

        public async Task<List<LocationResponseModel>> GetAllAsync()
        {
            var locations = mapper.Map<List<LocationResponseModel>>(await locationRepository.GetAllAsync());
            return locations;
        }

        public async Task<bool> UpdateAsync(int currentUserId, LocationEditModel location)
        {
            var locationDto = mapper.Map<LocationDto>(location);
            return await locationRepository.UpdateAsync(currentUserId, locationDto);
        }
    }
}

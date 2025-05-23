using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ActivityService(IMapper mapper, IActivityRepository activityRepository) : IActivityService
    {
        public async Task<int> CreateAsync(int currentUserId, ActivityEditModel activity)
        {
            var activityDto = mapper.Map<ActivityDto>(activity);
            return await activityRepository.CreateAsync(currentUserId, activityDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int activityId)
        {
            return await activityRepository.DeleteAsync(currentUserId, activityId);
        }

        public async Task<List<ActivityResponseModel>> GetAllAsync()
        {
            var activities = mapper.Map<List<ActivityResponseModel>>(await activityRepository.GetAllAsync());
            return activities;
        }

        public async Task<bool> UpdateAsync(int currentUserId, ActivityEditModel activity)
        {
            var activityDto = mapper.Map<ActivityDto>(activity);
            return await activityRepository.UpdateAsync(currentUserId, activityDto);
        }
    }
}

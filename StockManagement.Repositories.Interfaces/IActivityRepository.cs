using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task<int> CreateAsync(int currentUserId, ActivityDto activityDto);
        Task<bool> DeleteAsync(int currentUserId, int activityId);
        Task<bool> UpdateAsync(int currentUserId, ActivityDto activityDto);
        Task<List<ActivityDto>> GetAllAsync();
    }
}

using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface IActivityService
    {
        Task<int> CreateAsync(int currentUserId, ActivityEditModel activity);
        Task<bool> DeleteAsync(int currentUserId, int activityId);
        Task<List<ActivityResponseModel>> GetAllAsync();
        Task<ActivityFilteredResponseModel> GetFilteredAsync(ActivityFilterModel activityFilterModel);
        Task<bool> UpdateAsync(int currentUserId, ActivityEditModel activity);
    }
}

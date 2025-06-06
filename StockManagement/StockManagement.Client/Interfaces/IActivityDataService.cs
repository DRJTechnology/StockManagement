using StockManagement.Models;

namespace StockManagement.Client.Interfaces
{
    public interface IActivityDataService : IGenericDataService<ActivityEditModel, ActivityResponseModel>
    {
        Task<ActivityFilteredResponseModel> GetFilteredAsync(ActivityFilterModel activityFilterModel);
    }
}

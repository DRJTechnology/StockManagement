using StockManagement.Client.Interfaces;
using StockManagement.Models;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class ActivityDataService : GenericDataService<ActivityEditModel, ActivityResponseModel>, IActivityDataService
    {
        public ActivityDataService(HttpClient httpClient, ErrorNotificationService errorService)
            : base(httpClient, errorService)
        {
            ApiControllerName = "Activity";
        }

        public async Task<ActivityFilteredResponseModel> GetFilteredAsync(ActivityFilterModel activityFilterModel)
        {
            try
            {
                var query = new List<string>();

                if (activityFilterModel.Date.HasValue)
                    query.Add($"Date={activityFilterModel.Date.Value:yyyy-MM-dd}");
                if (activityFilterModel.ProductTypeId.HasValue)
                    query.Add($"ProductTypeId={activityFilterModel.ProductTypeId}");
                if (activityFilterModel.ProductId.HasValue)
                    query.Add($"ProductId={activityFilterModel.ProductId}");
                if (activityFilterModel.LocationId.HasValue)
                    query.Add($"LocationId={activityFilterModel.LocationId}");
                if (activityFilterModel.ActionId.HasValue)
                    query.Add($"ActionId={activityFilterModel.ActionId}");
                if (activityFilterModel.Quantity.HasValue)
                    query.Add($"Quantity={activityFilterModel.Quantity}");
                query.Add($"CurrentPage={activityFilterModel.CurrentPage}");
                query.Add($"PageSize={activityFilterModel.PageSize}");

                var queryString = string.Join("&", query);

                var url = $"api/{ApiControllerName}/GetFiltered?{queryString}";

                return await httpClient.GetFromJsonAsync<ActivityFilteredResponseModel>(url);
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }
    }
}

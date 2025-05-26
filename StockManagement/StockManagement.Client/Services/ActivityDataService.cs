using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class ActivityDataService : GenericDataService<ActivityEditModel, ActivityResponseModel>, IActivityDataService
    {
        public ActivityDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Activity";
        }
    }
}

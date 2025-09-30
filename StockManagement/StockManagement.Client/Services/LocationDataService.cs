using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class LocationDataService : GenericDataService<LocationEditModel, LocationResponseModel>, ILocationDataService
    {
        public LocationDataService(HttpClient httpClient, ErrorNotificationService errorService)
            : base(httpClient, errorService)
        {
            ApiControllerName = "Location";
        }
    }
}

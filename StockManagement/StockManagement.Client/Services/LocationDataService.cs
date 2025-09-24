using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class LocationDataService : GenericDataService<LocationEditModel, LocationResponseModel>, ILocationDataService
    {
        public LocationDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Location";
        }
    }
}

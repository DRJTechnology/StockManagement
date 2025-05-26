using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class VenueDataService : GenericDataService<VenueEditModel, VenueResponseModel>, IVenueDataService
    {
        public VenueDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Venue";
        }
    }
}

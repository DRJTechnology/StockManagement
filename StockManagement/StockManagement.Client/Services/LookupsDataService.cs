using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class LookupsDataService : GenericDataService<LookupsModel, LookupsModel>, ILookupsDataService
    {
        public LookupsDataService(HttpClient httpClient, ErrorNotificationService errorService)
            : base(httpClient, errorService)
        {
            ApiControllerName = "Lookups";
        }
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class LookupsDataService : GenericDataService<LookupsModel, LookupsModel>, ILookupsDataService
    {
        public LookupsDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Lookups";
        }
    }
}

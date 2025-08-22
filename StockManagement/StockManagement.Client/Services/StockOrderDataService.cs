using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class StockOrderDataService : GenericDataService<StockOrderEditModel, StockOrderResponseModel>, IStockOrderDataService
    {
        public StockOrderDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "StockOrder";
        }
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class StockSaleDataService : GenericDataService<StockSaleEditModel, StockSaleResponseModel>, IStockSaleDataService
    {
        public StockSaleDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "StockSale";
        }
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class StockSaleDetailDataService : GenericDataService<StockSaleDetailEditModel, StockSaleDetailResponseModel>, IStockSaleDetailDataService
    {
        public StockSaleDetailDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "StockSaleDetail";
        }
    }
}

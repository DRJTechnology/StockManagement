using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class StockOrderDetailDataService : GenericDataService<StockOrderDetailEditModel, StockOrderDetailResponseModel>, IStockOrderDetailDataService
    {
        public StockOrderDetailDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "StockOrderDetail";
        }
    }
}

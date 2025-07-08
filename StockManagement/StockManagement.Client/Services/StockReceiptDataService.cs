using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class StockReceiptDataService : GenericDataService<StockReceiptEditModel, StockReceiptResponseModel>, IStockReceiptDataService
    {
        public StockReceiptDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "StockReceipt";
        }
    }
}

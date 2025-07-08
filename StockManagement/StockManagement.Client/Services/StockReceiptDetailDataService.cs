using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class StockReceiptDetailDataService : GenericDataService<StockReceiptDetailEditModel, StockReceiptDetailResponseModel>, IStockReceiptDetailDataService
    {
        public StockReceiptDetailDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "StockReceiptDetail";
        }
    }
}

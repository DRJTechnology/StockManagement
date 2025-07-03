using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class DeliveryNoteDetailDataService : GenericDataService<DeliveryNoteDetailEditModel, DeliveryNoteDetailResponseModel>, IDeliveryNoteDetailDataService
    {
        public DeliveryNoteDetailDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "DeliveryNoteDetail";
        }
    }
}

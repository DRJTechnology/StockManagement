using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class DeliveryNoteDataService : GenericDataService<DeliveryNoteEditModel, DeliveryNoteResponseModel>, IDeliveryNoteDataService
    {
        public DeliveryNoteDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "DeliveryNote";
        }
    }
}

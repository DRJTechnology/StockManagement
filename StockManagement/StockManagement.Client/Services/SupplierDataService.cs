using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class SupplierDataService : GenericDataService<SupplierEditModel, SupplierResponseModel>, ISupplierDataService
    {
        public SupplierDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Supplier";
        }
    }
}

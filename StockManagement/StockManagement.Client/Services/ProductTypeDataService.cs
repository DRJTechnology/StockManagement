using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class ProductTypeDataService : GenericDataService<ProductTypeEditModel, ProductTypeResponseModel>, IProductTypeDataService
    {
        public ProductTypeDataService(HttpClient httpClient, ErrorNotificationService errorService)
            : base(httpClient, errorService)
        {
            ApiControllerName = "ProductType";
        }
    }
}

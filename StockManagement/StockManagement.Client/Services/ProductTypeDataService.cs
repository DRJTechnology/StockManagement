using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class ProductTypeDataService : GenericDataService<ProductTypeEditModel, ProductTypeResponseModel>, IProductTypeDataService
    {
        public ProductTypeDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "ProductType";
        }
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.Client.Services
{
    public class ProductDataService : GenericDataService<ProductEditModel, ProductResponseModel>, IProductDataService
    {
        public ProductDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "Product";
        }
    }
}

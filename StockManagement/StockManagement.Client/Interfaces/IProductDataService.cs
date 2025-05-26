using StockManagement.Models;

namespace StockManagement.Client.Interfaces
{
    public interface IProductDataService : IGenericDataService<ProductEditModel, ProductResponseModel>
    {
    }
}

using StockManagement.Models;

namespace StockManagement.Client.Interfaces
{
    public interface IStockOrderDataService : IGenericDataService<StockOrderEditModel, StockOrderResponseModel>
    {
    }
}

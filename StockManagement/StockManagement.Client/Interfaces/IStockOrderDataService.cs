using StockManagement.Models;

namespace StockManagement.Client.Interfaces
{
    public interface IStockOrderDataService : IGenericDataService<StockOrderEditModel, StockOrderResponseModel>
    {
        Task<bool> CreateStockOrderPayments(StockOrderPaymentsCreateModel stockOrderDetailPayments);
        Task<bool> MarkStockReceived(int stockOrderId);
    }
}

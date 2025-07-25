using StockManagement.Models.Dto.Reports;

namespace StockManagement.Client.Interfaces
{
    public interface IReportDataService
    {
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int locationId, int productTypeId, int productId);
        Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId);
    }
}

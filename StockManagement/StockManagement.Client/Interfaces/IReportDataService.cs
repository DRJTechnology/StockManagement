using StockManagement.Models.Dto.Reports;

namespace StockManagement.Client.Interfaces
{
    public interface IReportDataService
    {
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int venueId, int productTypeId, int productId);
        Task<List<StockReportItemDto>> GetStockReportAsync(int venueId, int productTypeId, int productId);
    }
}

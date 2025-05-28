using StockManagement.Models.Dto.Reports;

namespace StockManagement.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<StockReportItemDto>> GetStockReportAsync(int venueId, int productTypeId, int productId);
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int venueId, int productTypeId, int productId);
    }
}

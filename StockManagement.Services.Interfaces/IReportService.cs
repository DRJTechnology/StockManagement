using StockManagement.Models.Dto.Reports;

namespace StockManagement.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<StockReportItemDto>> GetStockReportAsync();
        Task<List<SalesReportItemDto>> GetSalesReportAsync();
    }
}

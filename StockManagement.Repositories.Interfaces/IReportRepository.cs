using StockManagement.Models.Dto.Reports;

namespace StockManagement.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<List<StockReportItemDto>> GetStockReportAsync();
        Task<List<SalesReportItemDto>> GetSalesReportAsync();
    }
}

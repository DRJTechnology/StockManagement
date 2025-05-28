using StockManagement.Models.Dto.Reports;

namespace StockManagement.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<List<StockReportItemDto>> GetStockReportAsync(int venueId, int productTypeId, int productId);
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int venueId, int productTypeId, int productId);
    }
}

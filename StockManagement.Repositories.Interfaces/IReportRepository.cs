using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId);
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int salesReportType, int locationId, int customerId, int productTypeId, int productId);
        Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync();
        Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync();
        Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync();
        Task<InventoryValueDto> GetInventoryValueReportAsync();
    }
}

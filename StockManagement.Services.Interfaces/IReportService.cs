using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId);
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int locationId, int productTypeId, int productId);
        Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync();
        Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync();
        Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync();
        Task<InventoryValueDto> GetInventoryValueReportAsync();
    }
}

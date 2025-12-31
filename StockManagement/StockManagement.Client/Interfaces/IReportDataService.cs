using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.Client.Interfaces
{
    public interface IReportDataService
    {
        Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync();
        Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync();
        Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync();
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int locationId, int productTypeId, int productId);
        Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId);
        Task<InventoryValueDto> GetInventoryValueReportAsync();
    }
}

using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId);
        Task<List<SalesReportItemDto>> GetSalesReportAsync(int salesReportType, int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate);
        Task<List<SalesInsightItemDto>> GetSalesInsightsAsync(int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate);
        Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis);
        Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync(DateTime fromDate, DateTime toDate);
        Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis);
        Task<InventoryValueDto> GetInventoryValueReportAsync();
        Task<List<StockValuationDto>> GetStockValuationReportAsync(DateTime asAtDate, int locationId, int productTypeId, int productId);
        Task<List<StockReconciliationDto>> GetStockReconciliationReportAsync(DateTime fromDate, DateTime toDate);
        Task<List<NominalLedgerDto>> GetNominalLedgerReportAsync(DateTime fromDate, DateTime toDate, int accountId);
        Task<List<IncomeExpenditureDto>> GetIncomeExpenditureReportAsync(DateTime fromDate, DateTime toDate, int sectionId);
        Task<List<OwnersAccountDto>> GetOwnersAccountReportAsync(DateTime fromDate, DateTime toDate);
        Task<List<YearEndCheckDto>> GetYearEndChecksReportAsync(DateTime fromDate, DateTime toDate);
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.ClientDataServices
{
    /// <summary>
    /// Server-side counterpart used during prerender. The report pages are
    /// WebAssembly-only and fetch their data after the client has started, so
    /// none of these are called - see the client data service duality note in
    /// CLAUDE.md.
    /// </summary>
    public class ClientReportDataService : IReportDataService
    {
        public Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
        {
            throw new NotImplementedException();
        }

        public Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
        {
            throw new NotImplementedException();
        }

        public Task<InventoryValueDto> GetInventoryValueReportAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesReportItemDto>> GetSalesReportAsync(int salesReportType, int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesInsightItemDto>> GetSalesInsightsAsync(int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockValuationDto>> GetStockValuationReportAsync(DateTime asAtDate, int locationId, int productTypeId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockReconciliationDto>> GetStockReconciliationReportAsync(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<List<NominalLedgerDto>> GetNominalLedgerReportAsync(DateTime fromDate, DateTime toDate, int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<IncomeExpenditureDto>> GetIncomeExpenditureReportAsync(DateTime fromDate, DateTime toDate, int sectionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OwnersAccountDto>> GetOwnersAccountReportAsync(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<List<YearEndCheckDto>> GetYearEndChecksReportAsync(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}

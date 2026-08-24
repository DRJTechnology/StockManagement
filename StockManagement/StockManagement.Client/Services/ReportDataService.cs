using StockManagement.Client.Interfaces;
using StockManagement.Client.Pages;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public class ReportDataService : IReportDataService
    {
        protected HttpClient httpClient { get; }
        protected ErrorNotificationService ErrorService { get; }
        protected string ApiControllerName { get; set; } = "Report";

        public ReportDataService(HttpClient httpClient, ErrorNotificationService errorService)
        {
            this.httpClient = httpClient;
            ErrorService = errorService;
        }

        // Report dates go on the query string in ISO form so they are read the
        // same way whatever the culture at either end. The app runs in en-GB,
        // where "05/04/2026" would otherwise be ambiguous.
        private static string Iso(DateTime date) => date.ToString("yyyy-MM-dd");

        private async Task<T> GetAsync<T>(string relativeUrl)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<T>($"api/{ApiControllerName}/{relativeUrl}");
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public Task<List<SalesReportItemDto>> GetSalesReportAsync(int salesReportType, int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
            => GetAsync<List<SalesReportItemDto>>($"sales?salesReportType={salesReportType}&locationId={locationId}&customerId={customerId}&productTypeId={productTypeId}&productId={productId}&fromDate={Iso(fromDate)}&toDate={Iso(toDate)}");

        public Task<List<SalesInsightItemDto>> GetSalesInsightsAsync(int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
            => GetAsync<List<SalesInsightItemDto>>($"salesinsights?locationId={locationId}&customerId={customerId}&productTypeId={productTypeId}&productId={productId}&fromDate={Iso(fromDate)}&toDate={Iso(toDate)}");

        public Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
            => GetAsync<List<StockReportItemDto>>($"stock?locationId={locationId}&productTypeId={productTypeId}&productId={productId}");

        public Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
            => GetAsync<List<BalanceSheetDto>>($"balancesheet?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}&basis={(int)basis}");

        public Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync(DateTime fromDate, DateTime toDate)
            => GetAsync<List<TrialBalanceDto>>($"trialbalance?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}");

        public Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
            => GetAsync<List<ProfitAndLossDto>>($"profitandloss?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}&basis={(int)basis}");

        public Task<InventoryValueDto> GetInventoryValueReportAsync()
            => GetAsync<InventoryValueDto>("inventoryvalue");

        public Task<List<StockValuationDto>> GetStockValuationReportAsync(DateTime asAtDate, int locationId, int productTypeId, int productId)
            => GetAsync<List<StockValuationDto>>($"stockvaluation?asAtDate={Iso(asAtDate)}&locationId={locationId}&productTypeId={productTypeId}&productId={productId}");

        public Task<List<StockReconciliationDto>> GetStockReconciliationReportAsync(DateTime fromDate, DateTime toDate)
            => GetAsync<List<StockReconciliationDto>>($"stockreconciliation?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}");

        public Task<List<NominalLedgerDto>> GetNominalLedgerReportAsync(DateTime fromDate, DateTime toDate, int accountId)
            => GetAsync<List<NominalLedgerDto>>($"nominalledger?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}&accountId={accountId}");

        public Task<List<IncomeExpenditureDto>> GetIncomeExpenditureReportAsync(DateTime fromDate, DateTime toDate, int sectionId)
            => GetAsync<List<IncomeExpenditureDto>>($"incomeexpenditure?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}&sectionId={sectionId}");

        public Task<List<OwnersAccountDto>> GetOwnersAccountReportAsync(DateTime fromDate, DateTime toDate)
            => GetAsync<List<OwnersAccountDto>>($"ownersaccount?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}");

        public Task<List<YearEndCheckDto>> GetYearEndChecksReportAsync(DateTime fromDate, DateTime toDate)
            => GetAsync<List<YearEndCheckDto>>($"yearendchecks?fromDate={Iso(fromDate)}&toDate={Iso(toDate)}");
    }
}

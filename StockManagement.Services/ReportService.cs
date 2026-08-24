using AutoMapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ReportService(IMapper mapper, IReportRepository reportRepository) : IReportService
    {
        public async Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
        {
            return await reportRepository.GetBalanceSheetReportAsync(fromDate, toDate, basis);
        }

        public async Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync(DateTime fromDate, DateTime toDate)
        {
            return await reportRepository.GetTrialBalanceReportAsync(fromDate, toDate);
        }

        public async Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
        {
            return await reportRepository.GetProfitAndLossReportAsync(fromDate, toDate, basis);
        }

        public async Task<InventoryValueDto> GetInventoryValueReportAsync()
        {
            var inventoryValue = await reportRepository.GetInventoryValueReportAsync();
            return inventoryValue;
        }

        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int salesReportType, int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
        {
            var reportItems = mapper.Map<List<SalesReportItemDto>>(await reportRepository.GetSalesReportAsync(salesReportType, locationId, customerId, productTypeId, productId, fromDate, toDate));
            return reportItems;
        }

        /// <summary>
        /// Sale lines at their natural grain. Returned as they come back from
        /// the database - the Insights page does its own aggregating.
        /// </summary>
        public async Task<List<SalesInsightItemDto>> GetSalesInsightsAsync(int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
        {
            return await reportRepository.GetSalesInsightsAsync(locationId, customerId, productTypeId, productId, fromDate, toDate);
        }

        public async Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
        {
            var reportItems = mapper.Map<List<StockReportItemDto>>(await reportRepository.GetStockReportAsync(locationId, productTypeId, productId));
            return reportItems;
        }

        public async Task<List<StockValuationDto>> GetStockValuationReportAsync(DateTime asAtDate, int locationId, int productTypeId, int productId)
        {
            return await reportRepository.GetStockValuationReportAsync(asAtDate, locationId, productTypeId, productId);
        }

        public async Task<List<StockReconciliationDto>> GetStockReconciliationReportAsync(DateTime fromDate, DateTime toDate)
        {
            return await reportRepository.GetStockReconciliationReportAsync(fromDate, toDate);
        }

        public async Task<List<NominalLedgerDto>> GetNominalLedgerReportAsync(DateTime fromDate, DateTime toDate, int accountId)
        {
            return await reportRepository.GetNominalLedgerReportAsync(fromDate, toDate, accountId);
        }

        public async Task<List<IncomeExpenditureDto>> GetIncomeExpenditureReportAsync(DateTime fromDate, DateTime toDate, int sectionId)
        {
            return await reportRepository.GetIncomeExpenditureReportAsync(fromDate, toDate, sectionId);
        }

        public async Task<List<OwnersAccountDto>> GetOwnersAccountReportAsync(DateTime fromDate, DateTime toDate)
        {
            return await reportRepository.GetOwnersAccountReportAsync(fromDate, toDate);
        }

        public async Task<List<YearEndCheckDto>> GetYearEndChecksReportAsync(DateTime fromDate, DateTime toDate)
        {
            return await reportRepository.GetYearEndChecksReportAsync(fromDate, toDate);
        }
    }
}

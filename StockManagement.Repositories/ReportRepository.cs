using Dapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class ReportRepository(IDbConnection dbConnection) : IReportRepository
    {
        // Report dates are calendar dates, not instants. Passing them as DbType.Date
        // keeps any stray time component out of the comparison, which is what
        // pulled transactions into the wrong financial year before.
        private static void AddDate(DynamicParameters parameters, string name, DateTime value)
            => parameters.Add(name, value.Date, DbType.Date);

        public async Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LocationId", locationId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);

            var reportItemList = await dbConnection.QueryAsync<StockReportItemDto>("dbo.Report_Stock", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int salesReportType, int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SalesReportType", salesReportType); // 1 = By Location, 2 = By Customer
            parameters.Add("@LocationId", locationId);
            parameters.Add("@CustomerId", customerId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);

            var reportItemList = await dbConnection.QueryAsync<SalesReportItemDto>("dbo.Report_Sales", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<SalesInsightItemDto>> GetSalesInsightsAsync(int locationId, int customerId, int productTypeId, int productId, DateTime fromDate, DateTime toDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LocationId", locationId);
            parameters.Add("@CustomerId", customerId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);

            var reportItemList = await dbConnection.QueryAsync<SalesInsightItemDto>("dbo.Report_SalesInsights", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);
            parameters.Add("@Basis", (byte)basis, DbType.Byte);

            var reportItemList = await dbConnection.QueryAsync<BalanceSheetDto>("[finance].[Report_BalanceSheet]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<TrialBalanceDto>> GetTrialBalanceReportAsync(DateTime fromDate, DateTime toDate)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);

            var reportItemList = await dbConnection.QueryAsync<TrialBalanceDto>("[finance].[Report_TrialBalance]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<ProfitAndLossDto>> GetProfitAndLossReportAsync(DateTime fromDate, DateTime toDate, AccountingBasis basis)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);
            parameters.Add("@Basis", (byte)basis, DbType.Byte);

            var reportItemList = await dbConnection.QueryAsync<ProfitAndLossDto>("[finance].[Report_ProfitAndLoss]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<InventoryValueDto> GetInventoryValueReportAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TotalActiveValue", dbType: DbType.Currency, direction: ParameterDirection.Output);
            parameters.Add("@TotalValue", dbType: DbType.Currency, direction: ParameterDirection.Output);

            await dbConnection.ExecuteAsync("[finance].[Report_InventoryValue]", parameters, commandType: CommandType.StoredProcedure);

            var inventoryValueDto = new InventoryValueDto
            {
                TotalValue = parameters.Get<decimal>("@TotalValue"),
                TotalActiveValue = parameters.Get<decimal>("@TotalActiveValue"),
            };
            return inventoryValueDto;
        }

        public async Task<List<StockValuationDto>> GetStockValuationReportAsync(DateTime asAtDate, int locationId, int productTypeId, int productId)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@AsAtDate", asAtDate);
            parameters.Add("@LocationId", locationId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);

            var reportItemList = await dbConnection.QueryAsync<StockValuationDto>("[finance].[Report_StockValuation]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<StockReconciliationDto>> GetStockReconciliationReportAsync(DateTime fromDate, DateTime toDate)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);

            var reportItemList = await dbConnection.QueryAsync<StockReconciliationDto>("[finance].[Report_StockReconciliation]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<NominalLedgerDto>> GetNominalLedgerReportAsync(DateTime fromDate, DateTime toDate, int accountId)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);
            parameters.Add("@AccountId", accountId);

            var reportItemList = await dbConnection.QueryAsync<NominalLedgerDto>("[finance].[Report_NominalLedger]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<IncomeExpenditureDto>> GetIncomeExpenditureReportAsync(DateTime fromDate, DateTime toDate, int sectionId)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);
            parameters.Add("@SectionId", (byte)sectionId, DbType.Byte);

            var reportItemList = await dbConnection.QueryAsync<IncomeExpenditureDto>("[finance].[Report_IncomeExpenditure]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<OwnersAccountDto>> GetOwnersAccountReportAsync(DateTime fromDate, DateTime toDate)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);

            var reportItemList = await dbConnection.QueryAsync<OwnersAccountDto>("[finance].[Report_OwnersAccount]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }

        public async Task<List<YearEndCheckDto>> GetYearEndChecksReportAsync(DateTime fromDate, DateTime toDate)
        {
            var parameters = new DynamicParameters();
            AddDate(parameters, "@FromDate", fromDate);
            AddDate(parameters, "@ToDate", toDate);

            var reportItemList = await dbConnection.QueryAsync<YearEndCheckDto>("[finance].[Report_YearEndChecks]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.ToList();
        }
    }
}

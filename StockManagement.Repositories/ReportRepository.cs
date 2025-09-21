using Dapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class ReportRepository(IDbConnection dbConnection) : IReportRepository
    {
        public async Task<List<StockReportItemDto>> GetStockReportAsync(int locationId, int productTypeId, int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LocationId", locationId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);

            var reportItemList = await dbConnection.QueryAsync<StockReportItemDto>("dbo.Report_Stock", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<StockReportItemDto>().ToList(); ;
        }
        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int locationId, int productTypeId, int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LocationId", locationId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);

            var reportItemList = await dbConnection.QueryAsync<SalesReportItemDto>("dbo.Report_Sales", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<SalesReportItemDto>().ToList(); ;
        }
        public async Task<List<BalanceSheetDto>> GetBalanceSheetReportAsync()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@FromDate", fromDate);
            //parameters.Add("@ToDate", toDate);

            var reportItemList = await dbConnection.QueryAsync<BalanceSheetDto>("[finance].[Report_BalanceSheet]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<BalanceSheetDto>().ToList(); ;
        }
        public async Task<decimal> GetInventoryValueReportAsync()
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@TotalValue", dbType: DbType.Currency, direction: ParameterDirection.Output);
                //parameters.Add("@TotalAmount", dbType: DbType.Currency, direction: ParameterDirection.Output);

                await dbConnection.ExecuteAsync("[finance].[Report_InventoryValue]", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<decimal>("@TotalValue");
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}

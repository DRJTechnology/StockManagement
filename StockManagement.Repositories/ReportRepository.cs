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

            var reportItemList = await dbConnection.QueryAsync<BalanceSheetDto>("[finance].[ReportBalanceSheet]", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<BalanceSheetDto>().ToList(); ;
        }
    }
}

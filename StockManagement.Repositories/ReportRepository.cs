using Dapper;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class ReportRepository(IDbConnection dbConnection) : IReportRepository
    {
        public async Task<List<StockReportItemDto>> GetStockReportAsync()
        {
            var parameters = new DynamicParameters();

            var reportItemList = await dbConnection.QueryAsync<StockReportItemDto>("dbo.Report_Stock", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<StockReportItemDto>().ToList(); ;
        }
        public async Task<List<SalesReportItemDto>> GetSalesReportAsync()
        {
            var parameters = new DynamicParameters();

            var reportItemList = await dbConnection.QueryAsync<SalesReportItemDto>("dbo.Report_Sales", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<SalesReportItemDto>().ToList(); ;
        }
    }
}

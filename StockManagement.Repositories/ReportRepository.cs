﻿using Dapper;
using StockManagement.Models.Dto.Reports;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class ReportRepository(IDbConnection dbConnection) : IReportRepository
    {
        public async Task<List<StockReportItemDto>> GetStockReportAsync(int venueId, int productTypeId, int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VenueId", venueId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);

            var reportItemList = await dbConnection.QueryAsync<StockReportItemDto>("dbo.Report_Stock", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<StockReportItemDto>().ToList(); ;
        }
        public async Task<List<SalesReportItemDto>> GetSalesReportAsync(int venueId, int productTypeId, int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VenueId", venueId);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@ProductId", productId);

            var reportItemList = await dbConnection.QueryAsync<SalesReportItemDto>("dbo.Report_Sales", parameters, commandType: CommandType.StoredProcedure);
            return reportItemList.Cast<SalesReportItemDto>().ToList(); ;
        }
    }
}

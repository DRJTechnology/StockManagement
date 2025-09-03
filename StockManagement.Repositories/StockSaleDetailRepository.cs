using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class StockSaleDetailRepository(IDbConnection dbConnection) : IStockSaleDetailRepository
    {
        public async Task<int> CreateAsync(int currentUserId, StockSaleDetailDto stockSaleDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@StockSaleId", stockSaleDetailDto.StockSaleId);
            parameters.Add("@ProductId", stockSaleDetailDto.ProductId);
            parameters.Add("@ProductTypeId", stockSaleDetailDto.ProductTypeId);
            parameters.Add("@Quantity", stockSaleDetailDto.Quantity);
            parameters.Add("@Deleted", stockSaleDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockSaleDetail_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int stockSaleDetailId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockSaleDetailId", stockSaleDetailId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockSaleDetail_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockSaleDetailDto stockSaleDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", stockSaleDetailDto.Id);
            parameters.Add("@ProductId", stockSaleDetailDto.ProductId);
            parameters.Add("@ProductTypeId", stockSaleDetailDto.ProductTypeId);
            parameters.Add("@Quantity", stockSaleDetailDto.Quantity);
            parameters.Add("@Deleted", stockSaleDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockSaleDetail_Update", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}

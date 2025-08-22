using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class StockOrderDetailRepository(IDbConnection dbConnection) : IStockOrderDetailRepository
    {
        public async Task<int> CreateAsync(int currentUserId, StockOrderDetailDto StockOrderDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@StockOrderId", StockOrderDetailDto.StockOrderId);
            parameters.Add("@ProductId", StockOrderDetailDto.ProductId);
            parameters.Add("@ProductTypeId", StockOrderDetailDto.ProductTypeId);
            parameters.Add("@Quantity", StockOrderDetailDto.Quantity);
            parameters.Add("@Deleted", StockOrderDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrderDetail_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockOrderDetailId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockOrderDetailId", StockOrderDetailId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrderDetail_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockOrderDetailDto StockOrderDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", StockOrderDetailDto.Id);
            parameters.Add("@ProductId", StockOrderDetailDto.ProductId);
            parameters.Add("@ProductTypeId", StockOrderDetailDto.ProductTypeId);
            parameters.Add("@Quantity", StockOrderDetailDto.Quantity);
            parameters.Add("@Deleted", StockOrderDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrderDetail_Update", parameters, commandType: CommandType.StoredProcedure);

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

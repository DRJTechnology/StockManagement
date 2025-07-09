using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class StockReceiptDetailRepository(IDbConnection dbConnection) : IStockReceiptDetailRepository
    {
        public async Task<int> CreateAsync(int currentUserId, StockReceiptDetailDto StockReceiptDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@StockReceiptId", StockReceiptDetailDto.StockReceiptId);
            parameters.Add("@ProductId", StockReceiptDetailDto.ProductId);
            parameters.Add("@ProductTypeId", StockReceiptDetailDto.ProductTypeId);
            parameters.Add("@Quantity", StockReceiptDetailDto.Quantity);
            parameters.Add("@Deleted", StockReceiptDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockReceiptDetail_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockReceiptDetailId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockReceiptDetailId", StockReceiptDetailId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockReceiptDetail_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockReceiptDetailDto StockReceiptDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", StockReceiptDetailDto.Id);
            parameters.Add("@ProductId", StockReceiptDetailDto.ProductId);
            parameters.Add("@ProductTypeId", StockReceiptDetailDto.ProductTypeId);
            parameters.Add("@Quantity", StockReceiptDetailDto.Quantity);
            parameters.Add("@Deleted", StockReceiptDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockReceiptDetail_Update", parameters, commandType: CommandType.StoredProcedure);

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

using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class StockReceiptRepository(IDbConnection dbConnection) : IStockReceiptRepository
    {
        public async Task<int> CreateAsync(int currentUserId, StockReceiptDto StockReceiptDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Date", StockReceiptDto.Date);
                parameters.Add("@SupplierId", StockReceiptDto.ContactId);
                parameters.Add("@Deleted", StockReceiptDto.Deleted);
                parameters.Add("@CurrentUserId", currentUserId);

                await dbConnection.ExecuteAsync("dbo.StockReceipt_Create", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return parameters.Get<int>("@Id");
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockReceiptId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockReceiptId", StockReceiptId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockReceipt_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<StockReceiptDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();
            var StockReceipts = await dbConnection.QueryAsync<StockReceiptDto>("dbo.StockReceipt_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return StockReceipts.ToList();
        }

        public async Task<StockReceiptDto> GetByIdAsync(int StockReceiptId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", StockReceiptId);

            var StockReceipt = new StockReceiptDto();
            using (var multipleResults = await dbConnection.QueryMultipleAsync("dbo.StockReceipt_LoadById", parameters, commandType: CommandType.StoredProcedure))
            {
                StockReceipt = multipleResults.Read<StockReceiptDto>().FirstOrDefault();

                if (StockReceipt != null)
                {
                    StockReceipt.DetailList = multipleResults.Read<StockReceiptDetailDto>().ToList();
                }
                return StockReceipt!;
            }
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockReceiptDto StockReceiptDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", StockReceiptDto.Id);
            parameters.Add("@Date", StockReceiptDto.Date);
            parameters.Add("@SupplierId", StockReceiptDto.ContactId);
            parameters.Add("@Deleted", StockReceiptDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockReceipt_Update", parameters, commandType: CommandType.StoredProcedure);

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

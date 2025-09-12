using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class StockSaleRepository(IDbConnection dbConnection) : IStockSaleRepository
    {
        public async Task<int> CreateAsync(int currentUserId, StockSaleDto stockSaleDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Date", stockSaleDto.Date);
            parameters.Add("@LocationId", stockSaleDto.LocationId);
            parameters.Add("@ContactId", stockSaleDto.ContactId);
            parameters.Add("@Deleted", stockSaleDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockSale_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int stockSaleId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockSaleId", stockSaleId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockSale_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<StockSaleDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();
            var stockSales = await dbConnection.QueryAsync<StockSaleDto>("dbo.StockSale_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return stockSales.ToList();
        }

        public async Task<StockSaleDto> GetByIdAsync(int stockSaleId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", stockSaleId);

            var stockSale = new StockSaleDto();
            using (var multipleResults = await dbConnection.QueryMultipleAsync("dbo.StockSale_LoadById", parameters, commandType: CommandType.StoredProcedure))
            {
                stockSale = multipleResults.Read<StockSaleDto>().FirstOrDefault();

                if (stockSale != null)
                {
                    stockSale.DetailList = multipleResults.Read<StockSaleDetailDto>().ToList();
                }
                return stockSale!;
            }
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockSaleDto stockSaleDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", stockSaleDto.Id);
            parameters.Add("@Date", stockSaleDto.Date);
            parameters.Add("@LocationId", stockSaleDto.LocationId);
            //parameters.Add("@DirectSale", stockSaleDto.DirectSale);
            parameters.Add("@Deleted", stockSaleDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockSale_Update", parameters, commandType: CommandType.StoredProcedure);

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

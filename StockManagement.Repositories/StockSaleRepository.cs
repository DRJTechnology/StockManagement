using Dapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class StockSaleRepository(IDbConnection dbConnection, IErrorLogRepository errorLogRepository) : IStockSaleRepository
    {
        public async Task<bool> ConfirmStockSaleAsync(int currentUserId, StockSaleConfirmationModel stockSaleConfirmation)
        {
                var recordList = new DataTable();
                recordList.Columns.Add("UnitPrice", typeof(decimal));
                recordList.Columns.Add("StockSaleDetailId", typeof(int));
                recordList.Columns.Add("ProductId", typeof(int));
                recordList.Columns.Add("ProductTypeId", typeof(int));
                recordList.Columns.Add("Quantity", typeof(int));

                foreach (var item in stockSaleConfirmation.StockSaleDetails)
                {
                    recordList.Rows.Add(item.UnitPrice, item.Id, item.ProductId, item.ProductTypeId, item.Quantity);
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@StockSaleId", stockSaleConfirmation.StockSaleId);
                parameters.Add("@FromLocationId", stockSaleConfirmation.LocationId);
                parameters.Add("@ContactId", stockSaleConfirmation.ContactId);
                parameters.Add("@TotalPrice", stockSaleConfirmation.TotalPrice);
                parameters.Add("@StockSaleDetails", recordList.AsTableValuedParameter("[finance].[StockSaleConfirmTableType]"));
                parameters.Add("@CurrentUserId", currentUserId);

                var result = await dbConnection.ExecuteAsync("dbo.StockSale_ConfirmSale", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return true;
                }
                else
                {
                    throw new UnauthorizedAccessException("Failed to confirm stock sale");
                }
        }

        public async Task<bool> ConfirmStockSalePaymentAsync(int currentUserId, StockSaleConfirmPaymentModel stockSaleConfirmPaymentModel)
        {
                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@StockSaleId", stockSaleConfirmPaymentModel.StockSaleId);
                parameters.Add("@PaymentDate", stockSaleConfirmPaymentModel.PaymentDate);
                parameters.Add("@Description", stockSaleConfirmPaymentModel.Description);
                parameters.Add("@CurrentUserId", currentUserId);

                var result = await dbConnection.ExecuteAsync("dbo.StockSale_ConfirmPayment", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return true;
                }
                else
                {
                    throw new UnauthorizedAccessException("Failed to confirm stock sale payment");
                }
        }

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
                    throw new UnauthorizedAccessException("Failed to create stock sale");
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
                    throw new UnauthorizedAccessException("Failed to delete stock sale");
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

        public async Task<bool> ResetStockSaleAsync(int currentUserId, int stockSaleId)
        {
                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@StockSaleId", stockSaleId);
                parameters.Add("@CurrentUserId", currentUserId);

                var result = await dbConnection.ExecuteAsync("dbo.StockSale_ResetSale", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return true;
                }
                else
                {
                    throw new UnauthorizedAccessException("Failed to reset stock sale");
                }
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockSaleDto stockSaleDto)
        {
                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@Id", stockSaleDto.Id);
                parameters.Add("@Date", stockSaleDto.Date);
                parameters.Add("@LocationId", stockSaleDto.LocationId);
                parameters.Add("@ContactId", stockSaleDto.ContactId);
                parameters.Add("@Deleted", stockSaleDto.Deleted);
                parameters.Add("@CurrentUserId", currentUserId);

                await dbConnection.ExecuteAsync("dbo.StockSale_Update", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return true;
                }
                else
                {
                    throw new UnauthorizedAccessException("Failed to update stock sale");
                }
        }
    }
}

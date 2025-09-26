using Dapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class StockOrderRepository(IDbConnection dbConnection) : IStockOrderRepository
    {
        public async Task<int> CreateAsync(int currentUserId, StockOrderDto stockOrderDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Date", stockOrderDto.Date);
            parameters.Add("@ContactId", stockOrderDto.ContactId);
            parameters.Add("@Deleted", stockOrderDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrder_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

        }

        public async Task<bool> CreateStockOrderPayments(int currentUserId, StockOrderPaymentsCreateModel stockOrderDetailPayments)
        {
            var recordList = new DataTable();
            recordList.Columns.Add("UnitPrice", typeof(decimal));
            recordList.Columns.Add("StockOrderDetailId", typeof(int));
            recordList.Columns.Add("ProductId", typeof(int));
            recordList.Columns.Add("ProductTypeId", typeof(int));
            recordList.Columns.Add("Quantity", typeof(int));

            foreach (var item in stockOrderDetailPayments.StockOrderDetailPayments)
            {
                recordList.Rows.Add(item.UnitPrice, item.Id, item.ProductId, item.ProductTypeId, item.Quantity);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockOrderId", stockOrderDetailPayments.StockOrderId);
            parameters.Add("@ContactId", stockOrderDetailPayments.ContactId);
            parameters.Add("@Cost", stockOrderDetailPayments.Cost);
            parameters.Add("@Description", stockOrderDetailPayments.Description);
            parameters.Add("@PaymentDate", stockOrderDetailPayments.PaymentDate);
            parameters.Add("@StockPaymentDetails", recordList.AsTableValuedParameter("[finance].[StockPaymentTableType]"));
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrder_CreateStockOrderPayments", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int StockOrderId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockOrderId", StockOrderId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrder_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<StockOrderDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();
            var StockOrders = await dbConnection.QueryAsync<StockOrderDto>("dbo.StockOrder_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return StockOrders.ToList();
        }

        public async Task<StockOrderDto> GetByIdAsync(int StockOrderId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", StockOrderId);

            var StockOrder = new StockOrderDto();
            using (var multipleResults = await dbConnection.QueryMultipleAsync("dbo.StockOrder_LoadById", parameters, commandType: CommandType.StoredProcedure))
            {
                StockOrder = multipleResults.Read<StockOrderDto>().FirstOrDefault();

                if (StockOrder != null)
                {
                    StockOrder.DetailList = multipleResults.Read<StockOrderDetailDto>().ToList();
                }
                return StockOrder!;
            }
        }

        public async Task<bool> MarkStockReceivedAsync(int currentUserId, int stockOrderId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@StockOrderId", stockOrderId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrder_ActivateStock", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> UpdateAsync(int currentUserId, StockOrderDto StockOrderDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", StockOrderDto.Id);
            parameters.Add("@Date", StockOrderDto.Date);
            parameters.Add("@ContactId", StockOrderDto.ContactId);
            parameters.Add("@Deleted", StockOrderDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.StockOrder_Update", parameters, commandType: CommandType.StoredProcedure);

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

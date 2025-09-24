using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class DeliveryNoteDetailRepository(IDbConnection dbConnection) : IDeliveryNoteDetailRepository
    {
        public async Task<int> CreateAsync(int currentUserId, DeliveryNoteDetailDto deliveryNoteDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@DeliveryNoteId", deliveryNoteDetailDto.DeliveryNoteId);
            parameters.Add("@ProductId", deliveryNoteDetailDto.ProductId);
            parameters.Add("@ProductTypeId", deliveryNoteDetailDto.ProductTypeId);
            parameters.Add("@Quantity", deliveryNoteDetailDto.Quantity);
            parameters.Add("@Deleted", deliveryNoteDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.DeliveryNoteDetail_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int deliveryNoteDetailId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@DeliveryNoteDetailId", deliveryNoteDetailId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.DeliveryNoteDetail_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> UpdateAsync(int currentUserId, DeliveryNoteDetailDto deliveryNoteDetailDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", deliveryNoteDetailDto.Id);
            parameters.Add("@ProductId", deliveryNoteDetailDto.ProductId);
            parameters.Add("@ProductTypeId", deliveryNoteDetailDto.ProductTypeId);
            parameters.Add("@Quantity", deliveryNoteDetailDto.Quantity);
            parameters.Add("@Deleted", deliveryNoteDetailDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.DeliveryNoteDetail_Update", parameters, commandType: CommandType.StoredProcedure);

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

using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Models.Dto.Profile;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class DeliveryNoteRepository(IDbConnection dbConnection) : IDeliveryNoteRepository
    {
        public async Task<int> CreateAsync(int currentUserId, DeliveryNoteDto deliveryNoteDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Date", deliveryNoteDto.Date);
            parameters.Add("@VenueId", deliveryNoteDto.VenueId);
            parameters.Add("@Deleted", deliveryNoteDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.DeliveryNote_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int deliveryNoteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@DeliveryNoteId", deliveryNoteId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.DeliveryNote_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<DeliveryNoteDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();
            var deliveryNotes = await dbConnection.QueryAsync<DeliveryNoteDto>("dbo.DeliveryNote_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return deliveryNotes.ToList();
        }

        public async Task<DeliveryNoteDto> GetByIdAsync(int deliveryNoteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", deliveryNoteId);

            var deliveryNote = await dbConnection.QueryFirstOrDefaultAsync<DeliveryNoteDto>("dbo.DeliveryNote_LoadById", parameters, commandType: CommandType.StoredProcedure);
            return deliveryNote!;
        }

        public async Task<bool> UpdateAsync(int currentUserId, DeliveryNoteDto deliveryNoteDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", deliveryNoteDto.Id);
            parameters.Add("@Date", deliveryNoteDto.Date);
            parameters.Add("@VenueId", deliveryNoteDto.VenueId);
            parameters.Add("@Deleted", deliveryNoteDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.DeliveryNote_Update", parameters, commandType: CommandType.StoredProcedure);

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

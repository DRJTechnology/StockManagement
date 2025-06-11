using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class VenueRepository(IDbConnection dbConnection) : IVenueRepository
    {
        public async Task<int> CreateAsync(int currentUserId, VenueDto venueDto)
        {
            if (venueDto == null)
            {
                throw new ArgumentNullException(nameof(venueDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@VenueName", venueDto.VenueName);
            parameters.Add("@Notes", venueDto.Notes);
            parameters.Add("@Deleted", venueDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Venue_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int venueId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@VenueId", venueId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Venue_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<VenueDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var venueList = await dbConnection.QueryAsync<VenueDto>("dbo.Venue_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return venueList.Cast<VenueDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, VenueDto venueDto)
        {
            if (venueDto == null)
            {
                throw new ArgumentNullException(nameof(venueDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", venueDto.Id);
            parameters.Add("@VenueName", venueDto.VenueName);
            parameters.Add("@Notes", venueDto.Notes);
            parameters.Add("@Deleted", venueDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Venue_Update", parameters, commandType: CommandType.StoredProcedure);

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

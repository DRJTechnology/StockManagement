using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class LocationRepository(IDbConnection dbConnection) : ILocationRepository
    {
        public async Task<int> CreateAsync(int currentUserId, LocationDto locationDto)
        {
            if (locationDto == null)
            {
                throw new ArgumentNullException(nameof(locationDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Name", locationDto.Name);
            parameters.Add("@Notes", locationDto.Notes);
            parameters.Add("@Deleted", locationDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Location_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int locationId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@LocationId", locationId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Location_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<LocationDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var locationList = await dbConnection.QueryAsync<LocationDto>("dbo.Location_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return locationList.Cast<LocationDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, LocationDto locationDto)
        {
            if (locationDto == null)
            {
                throw new ArgumentNullException(nameof(locationDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", locationDto.Id);
            parameters.Add("@Name", locationDto.Name);
            parameters.Add("@Notes", locationDto.Notes);
            parameters.Add("@Deleted", locationDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Location_Update", parameters, commandType: CommandType.StoredProcedure);

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

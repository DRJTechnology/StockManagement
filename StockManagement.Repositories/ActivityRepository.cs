using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class ActivityRepository(IDbConnection dbConnection) : IActivityRepository
    {
        public async Task<int> CreateAsync(int currentUserId, ActivityDto activityDto)
        {
            if (activityDto == null)
            {
                throw new ArgumentNullException(nameof(activityDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@ActivityDate", activityDto.ActivityDate);
            parameters.Add("@ActionId", activityDto.ActionId);
            parameters.Add("@ProductId", activityDto.ProductId);
            parameters.Add("@ProductTypeId", activityDto.ProductTypeId);
            parameters.Add("@VenueId", activityDto.VenueId);
            parameters.Add("@Quantity", activityDto.Quantity);
            parameters.Add("@Deleted", activityDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Activity_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int activityId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@ActivityId", activityId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Activity_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<ActivityDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var activityList = await dbConnection.QueryAsync<ActivityDto>("dbo.Activity_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return activityList.Cast<ActivityDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, ActivityDto activityDto)
        {
            if (activityDto == null)
            {
                throw new ArgumentNullException(nameof(activityDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", activityDto.Id);
            parameters.Add("@ActivityDate", activityDto.ActivityDate);
            parameters.Add("@ActionId", activityDto.ActionId);
            parameters.Add("@ProductId", activityDto.ProductId);
            parameters.Add("@ProductTypeId", activityDto.ProductTypeId);
            parameters.Add("@VenueId", activityDto.VenueId);
            parameters.Add("@Quantity", activityDto.Quantity);
            parameters.Add("@Deleted", activityDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Activity_Update", parameters, commandType: CommandType.StoredProcedure);

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

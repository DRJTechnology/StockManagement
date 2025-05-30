using System.Data;
using System.Diagnostics;
using Dapper;
using StockManagement.Models;
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

        public async Task<ActivityFilteredResponseModel> GetFilteredAsync(ActivityFilterModel activityFilterModel)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ActivityDate", activityFilterModel.Date);
                parameters.Add("@ActionId", activityFilterModel.ActionId);
                parameters.Add("@ProductId", activityFilterModel.ProductId);
                parameters.Add("@ProductTypeId", activityFilterModel.ProductTypeId);
                parameters.Add("@VenueId", activityFilterModel.VenueId);
                parameters.Add("@Quantity", activityFilterModel.Quantity);
                parameters.Add("@CurrentPage", activityFilterModel.CurrentPage);
                parameters.Add("@PageSize", activityFilterModel.PageSize);
                parameters.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var filteredActivity = await dbConnection.QueryAsync<ActivityResponseModel>("dbo.Activity_LoadFiltered", parameters, commandType: CommandType.StoredProcedure);

                return new ActivityFilteredResponseModel()
                {
                    Activity = filteredActivity.Cast<ActivityResponseModel>().ToList(),
                    TotalPages = parameters.Get<int>("@TotalPages"),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetFilteredAsync: {ex.Message}");
                throw;
            }
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

using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class ActionRepository(IDbConnection dbConnection) : IActionRepository
    {
        public async Task<int> CreateAsync(int currentUserId, ActionDto actionDto)
        {
            if (actionDto == null)
            {
                throw new ArgumentNullException("actionDto");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@ActionName", actionDto.ActionName);
            parameters.Add("@Direction", actionDto.Direction);
            parameters.Add("@AffectStockRoom", actionDto.AffectStockRoom);
            parameters.Add("@Deleted", actionDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Action_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int actionId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@ActionId", actionId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Action_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<ActionDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var actionList = await dbConnection.QueryAsync<ActionDto>("dbo.Action_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return actionList.Cast<ActionDto>().ToList(); ;
        }

        public async Task<bool> UpdateAsync(int currentUserId, ActionDto actionDto)
        {
            if (actionDto == null)
            {
                throw new ArgumentNullException("actionDto");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", actionDto.Id);
            parameters.Add("@ActionName", actionDto.ActionName);
            parameters.Add("@Direction", actionDto.Direction);
            parameters.Add("@AffectStockRoom", actionDto.AffectStockRoom);
            parameters.Add("@Deleted", actionDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Action_Update", parameters, commandType: CommandType.StoredProcedure);

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

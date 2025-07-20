using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class SettingRepository(IDbConnection dbConnection) : ISettingRepository
    {
        public async Task<List<SettingDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var settingList = await dbConnection.QueryAsync<SettingDto>("dbo.Setting_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return settingList.Cast<SettingDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, SettingDto settingDto)
        {
            if (settingDto == null)
            {
                throw new ArgumentNullException(nameof(settingDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", settingDto.Id);
            parameters.Add("@Settingvalue", settingDto.SettingValue);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Setting_Update", parameters, commandType: CommandType.StoredProcedure);

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

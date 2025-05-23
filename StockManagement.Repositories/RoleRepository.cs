using Dapper;
using StockManagement.Models.Dto.Profile;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class RoleRepository(IDbConnection dbConnection) : IRoleRepository
    {
        public async Task<int> CreateRoleAsync(ApplicationRole role)
        {
            ArgumentNullException.ThrowIfNull(role);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Name", role.Name);
            parameters.Add("@NormalizedName", role.NormalizedName);
            parameters.Add("@ConcurrencyStamp", role.ConcurrencyStamp);

            await dbConnection.ExecuteAsync("auth.RoleCreate", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Id");
        }

        public async Task<ApplicationRole> GetByIdAsync(int roleId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", roleId);

            var role = await dbConnection.QueryFirstOrDefaultAsync<ApplicationRole>("auth.RoleLoadById", parameters, commandType: CommandType.StoredProcedure);
            return role!;
        }

        public async Task<ApplicationRole> GetByNameAsync(string normalizedName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@NormalizedName", normalizedName);

            var role = await dbConnection.QueryFirstOrDefaultAsync<ApplicationRole>("auth.RoleLoadByUserName", parameters, commandType: CommandType.StoredProcedure);
            return role!;
        }

        public async Task<IList<ApplicationRole>> GetByUserIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            var roles = await dbConnection.QueryAsync<ApplicationRole>("auth.RoleLoadByUserId", parameters, commandType: CommandType.StoredProcedure);
            return roles.ToList();
        }

        public async Task UpdateRole(ApplicationRole role)
        {
            ArgumentNullException.ThrowIfNull(role);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", role.Id);
            parameters.Add("@Name", role.Name);
            parameters.Add("@NormalizedName", role.NormalizedName);
            parameters.Add("@ConcurrencyStamp", role.ConcurrencyStamp);

            await dbConnection.ExecuteAsync("auth.RoleUpdate", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}

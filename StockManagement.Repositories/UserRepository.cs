using Dapper;
using Microsoft.AspNetCore.Identity;
using StockManagement.Models.Dto.Profile;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class UserRepository(IDbConnection dbConnection) : IUserRepository
    {
        public async Task<int> CreateUserAsync(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@UserName", user.UserName);
            parameters.Add("@NormalizedUserName", user.NormalizedUserName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@NormalizedEmail", user.NormalizedEmail);
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@EmailConfirmed", user.EmailConfirmed);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@SecurityStamp", user.SecurityStamp);
            parameters.Add("@ConcurrencyStamp", user.ConcurrencyStamp);
            parameters.Add("@PhoneNumber", user.PhoneNumber);
            parameters.Add("@PhoneNumberConfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@TwoFactorEnabled", user.TwoFactorEnabled);
            parameters.Add("@LockoutEnd", user.LockoutEnd);
            parameters.Add("@LockoutEnabled", user.LockoutEnabled);
            parameters.Add("@AccessFailedCount", user.AccessFailedCount);

            await dbConnection.ExecuteAsync("auth.UserCreate", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Id");
        }

        public async Task CreateUserLoginProviderAsync(string loginProvider, string providerKey, string providerDisplayName, int userId)
        {
            if (string.IsNullOrEmpty(loginProvider))
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }
            if (string.IsNullOrEmpty(providerKey))
            {
                throw new ArgumentNullException(nameof(providerKey));
            }
            if (string.IsNullOrEmpty(providerDisplayName))
            {
                throw new ArgumentNullException(nameof(providerDisplayName));
            }
            if (userId == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@LoginProvider", loginProvider);
            parameters.Add("@ProviderKey", providerKey);
            parameters.Add("@ProviderDisplayName", providerDisplayName);
            parameters.Add("@UserId", userId);

            await dbConnection.ExecuteAsync("auth.UserLoginProviderCreate", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            await dbConnection.ExecuteAsync("auth.UserDelete", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<ApplicationUser> GetByEmailAsync(string normalizedEmail)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@NormalizedEmailAddress", normalizedEmail);

            var user = await dbConnection.QueryFirstOrDefaultAsync<ApplicationUser>("auth.UserLoadByEmail", parameters, commandType: CommandType.StoredProcedure);
            return user!;
        }

        public async Task<ApplicationUser> GetByIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", userId);

            var user = await dbConnection.QueryFirstOrDefaultAsync<ApplicationUser>("auth.UserLoadById", parameters, commandType: CommandType.StoredProcedure);
            return user!;
        }

        public async Task<ApplicationUser?> GetByLoginProviderAsync(string loginProvider, string providerKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LoginProvider", loginProvider);
            parameters.Add("@ProviderKey", providerKey);

            var user = await dbConnection.QueryFirstOrDefaultAsync<ApplicationUser>("auth.UserLoadByLoginProvider", parameters, commandType: CommandType.StoredProcedure);
            return user!;
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string normalizedUserName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@NormalizedUserName", normalizedUserName);

            var user = await dbConnection.QueryFirstOrDefaultAsync<ApplicationUser>("auth.UserLoadByUserName", parameters, commandType: CommandType.StoredProcedure);
            return user!;
        }

        public async Task<IList<UserLoginInfo>> GetLoginProviderByUserIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            var loginProviders = await dbConnection.QueryAsync<ApplicationUserLoginInfo>("auth.UserLoginLoadByUserId", parameters, commandType: CommandType.StoredProcedure);
            return loginProviders.Cast<UserLoginInfo>().ToList();
        }

        public async Task<bool> IsUserInRoleAsync(int userId, string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Result", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@UserId", userId);
            parameters.Add("@RoleName", roleName);

            await dbConnection.ExecuteAsync("auth.UserIsInRole", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<bool>("@Result");
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", user.Id);
            parameters.Add("@UserName", user.UserName);
            parameters.Add("@NormalizedUserName", user.NormalizedUserName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@NormalizedEmail", user.NormalizedEmail);
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@EmailConfirmed", user.EmailConfirmed);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@SecurityStamp", user.SecurityStamp);
            parameters.Add("@ConcurrencyStamp", user.ConcurrencyStamp);
            parameters.Add("@PhoneNumber", user.PhoneNumber);
            parameters.Add("@PhoneNumberConfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@TwoFactorEnabled", user.TwoFactorEnabled);
            parameters.Add("@LockoutEnd", user.LockoutEnd);
            parameters.Add("@LockoutEnabled", user.LockoutEnabled);
            parameters.Add("@AccessFailedCount", user.AccessFailedCount);

            await dbConnection.ExecuteAsync("auth.UserUpdate", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}

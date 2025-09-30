using Dapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using System.Data;

namespace StockManagement.Repositories.Finance
{
    public class AccountRepository(IDbConnection dbConnection) : IAccountRepository
    {
        public async Task<int> CreateAsync(int currentUserId, AccountDto accountDto)
        {
            if (accountDto == null)
            {
                throw new ArgumentNullException(nameof(accountDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Name", accountDto.Name);
            parameters.Add("@Notes", accountDto.Notes);
            parameters.Add("@AccountTypeId", accountDto.AccountTypeId);
            parameters.Add("@Active", accountDto.Active);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("finance.Account_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int accountId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@AccountId", accountId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("finance.Account_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<AccountDto>> GetAllAsync(bool includeInactive)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ActiveOnly", !includeInactive);

            var accountList = await dbConnection.QueryAsync<AccountDto>("finance.Account_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return accountList.Cast<AccountDto>().ToList();
        }

        public async Task<List<AccountDto>> GetByTypeAsync(int accountTypeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AccountTypeId", accountTypeId);

            var accountList = await dbConnection.QueryAsync<AccountDto>("finance.Account_LoadByType", parameters, commandType: CommandType.StoredProcedure);
            return accountList.Cast<AccountDto>().ToList();
        }

        public async Task<AccountDto> GetByIdAsync(int accountId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", accountId);

            var account = await dbConnection.QueryFirstOrDefaultAsync<AccountDto>("finance.Account_LoadById", parameters, commandType: CommandType.StoredProcedure);
            return account!;
        }

        public async Task<bool> UpdateAsync(int currentUserId, AccountDto accountDto)
        {
            if (accountDto == null)
            {
                throw new ArgumentNullException(nameof(accountDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", accountDto.Id);
            parameters.Add("@AccountTypeId", accountDto.AccountTypeId);
            parameters.Add("@Name", accountDto.Name);
            parameters.Add("@Notes", accountDto.Notes);
            parameters.Add("@Active", accountDto.Active);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("finance.Account_Update", parameters, commandType: CommandType.StoredProcedure);

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

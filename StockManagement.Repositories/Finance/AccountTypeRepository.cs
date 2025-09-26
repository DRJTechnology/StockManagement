using Dapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using System.Data;

namespace StockManagement.Repositories.Finance
{
    public class AccountTypeRepository(IDbConnection dbConnection) : IAccountTypeRepository
    {
        public async Task<List<AccountTypeDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var productTypeList = await dbConnection.QueryAsync<AccountTypeDto>("finance.AccountType_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return productTypeList.Cast<AccountTypeDto>().ToList();
        }
    }
}

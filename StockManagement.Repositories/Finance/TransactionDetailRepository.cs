using Dapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using System.Data;
using System.Diagnostics;

namespace StockManagement.Repositories.Finance
{
    public class TransactionDetailRepository(IDbConnection dbConnection) : ITransactionDetailRepository
    {
        public async Task<List<TransactionDetailDto>> GetByAccountTypeAsync(int accountTypeId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AccountTypeId", accountTypeId);
                //parameters.Add("@CurrentPage", activityFilterModel.CurrentPage);
                //parameters.Add("@PageSize", activityFilterModel.PageSize);
                //parameters.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var transactions = await dbConnection.QueryAsync<TransactionDetailDto>("finance.TransactionDetail_LoadByAccountType", parameters, commandType: CommandType.StoredProcedure);

                return transactions.Cast<TransactionDetailDto>().ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetFilteredAsync: {ex.Message}");
                throw;
            }
        }
    }
}

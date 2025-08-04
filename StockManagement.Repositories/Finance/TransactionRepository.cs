using Dapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using System.Data;
using System.Diagnostics;

namespace StockManagement.Repositories.Finance
{
    public class TransactionRepository(IDbConnection dbConnection) : ITransactionRepository
    {
        public async Task<int> CreateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto)
        {
            try
            {
                if (transactionDetailDto == null)
                {
                    throw new ArgumentNullException(nameof(transactionDetailDto));
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@TransactionTypeId", transactionDetailDto.TransactionTypeId);
                parameters.Add("@AccountId", transactionDetailDto.AccountId);
                parameters.Add("@Date", transactionDetailDto.Date);
                parameters.Add("@Description", transactionDetailDto.Description);
                parameters.Add("@Amount", transactionDetailDto.Amount);
                parameters.Add("@ContactId", transactionDetailDto.ContactId);
                parameters.Add("@CurrentUserId", currentUserId);

                await dbConnection.ExecuteAsync("finance.Transaction_CreateExpenseIncome", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return parameters.Get<int>("@Id");
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByDetailIdAsync(int currentUserId, int transactionDetailId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@TransactionDetailId", transactionDetailId);
                parameters.Add("@CurrentUserId", currentUserId);

                await dbConnection.ExecuteAsync("finance.Transaction_DeleteByDetailId", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return true;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionDetailDto>> GetDetailByAccountTypeAsync(int accountTypeId)
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
                Debug.WriteLine($"Error in GetByAccountTypeAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto)
        {
            try
            {
                if (transactionDetailDto == null)
                {
                    throw new ArgumentNullException(nameof(transactionDetailDto));
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@TransactionTypeId", transactionDetailDto.TransactionTypeId);
                parameters.Add("@Id", transactionDetailDto.Id);
                parameters.Add("@AccountId", transactionDetailDto.AccountId);
                parameters.Add("@Date", transactionDetailDto.Date);
                parameters.Add("@Description", transactionDetailDto.Description);
                parameters.Add("@Amount", transactionDetailDto.Amount);
                parameters.Add("@ContactId", transactionDetailDto.ContactId);
                parameters.Add("@CurrentUserId", currentUserId);

                await dbConnection.ExecuteAsync("finance.Transaction_UpdateExpenseIncome", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return true;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

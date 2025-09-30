using AutoMapper;
using Dapper;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace StockManagement.Repositories.Finance
{
    public class TransactionRepository(IDbConnection dbConnection, IMapper mapper) : ITransactionRepository
    {
        public async Task<int> CreateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto)
        {
            if (transactionDetailDto == null)
            {
                throw new ArgumentNullException(nameof(transactionDetailDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@TransactionDetailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
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
                return parameters.Get<int>("@TransactionDetailId");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteByDetailIdAsync(int currentUserId, int transactionDetailId)
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

        public async Task<List<TransactionDetailDto>> GetDetailByAccountAsync(int accountId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AccountId", accountId);
            //parameters.Add("@CurrentPage", activityFilterModel.CurrentPage);
            //parameters.Add("@PageSize", activityFilterModel.PageSize);
            //parameters.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var transactions = await dbConnection.QueryAsync<TransactionDetailDto>("finance.TransactionDetail_LoadByAccount", parameters, commandType: CommandType.StoredProcedure);

            return transactions.Cast<TransactionDetailDto>().ToList();
        }

        public async Task<List<TransactionDetailDto>> GetDetailByAccountTypeAsync(int accountTypeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AccountTypeId", accountTypeId);
            //parameters.Add("@CurrentPage", activityFilterModel.CurrentPage);
            //parameters.Add("@PageSize", activityFilterModel.PageSize);
            //parameters.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var transactions = await dbConnection.QueryAsync<TransactionDetailDto>("finance.TransactionDetail_LoadByAccountType", parameters, commandType: CommandType.StoredProcedure);

            return transactions.Cast<TransactionDetailDto>().ToList();
        }

        public async Task<TransactionFilteredResponseModel> GetFilteredAsync(TransactionFilterModel transactionFilterModel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AccountId", transactionFilterModel.AccountId);
            parameters.Add("@ContactId", transactionFilterModel.ContactId);
            parameters.Add("@TransactionTypeId", transactionFilterModel.TransactionTypeId);
            parameters.Add("@FromDate", transactionFilterModel.FromDate);
            parameters.Add("@ToDate", transactionFilterModel.ToDate);
            parameters.Add("@PageSize", transactionFilterModel.PageSize);
            parameters.Add("@CurrentPage", transactionFilterModel.CurrentPage);
            parameters.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var filteredTransactionDetails = await dbConnection.QueryAsync<TransactionDetailDto>("finance.TransactionDetail_LoadFiltered", parameters, commandType: CommandType.StoredProcedure);

            return new TransactionFilteredResponseModel()
            {
                TransactionDetailList = mapper.Map<List<TransactionDetailResponseModel>>(filteredTransactionDetails.ToList()),
                TotalPages = parameters.Get<int>("@TotalPages"),
            };
        }

        public async Task<decimal> GetTotalAmountFilteredAsync(TransactionFilterModel transactionFilterModel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AccountId", transactionFilterModel.AccountId);
            parameters.Add("@ContactId", transactionFilterModel.ContactId);
            parameters.Add("@TransactionTypeId", transactionFilterModel.TransactionTypeId);
            parameters.Add("@FromDate", transactionFilterModel.FromDate);
            parameters.Add("@ToDate", transactionFilterModel.ToDate);
            parameters.Add("@TotalAmount", dbType: DbType.Currency, direction: ParameterDirection.Output);

            var response = await dbConnection.QueryAsync("finance.TransactionDetail_LoadTotalFiltered", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<decimal>("@TotalAmount");
        }

        public async Task<bool> UpdateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto)
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
    }
}

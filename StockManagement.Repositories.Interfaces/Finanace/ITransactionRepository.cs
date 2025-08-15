using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Finance;

namespace StockManagement.Repositories.Interfaces.Finanace
{
    public interface ITransactionRepository
    {
        Task<int> CreateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto);
        Task<bool> DeleteByDetailIdAsync(int currentUserId, int transactionDetailId);
        Task<List<TransactionDetailDto>> GetDetailByAccountAsync(int accountId);
        Task<List<TransactionDetailDto>> GetDetailByAccountTypeAsync(int accountTypeId);
        Task<TransactionFilteredResponseModel> GetFilteredAsync(TransactionFilterModel transactionFilterModel);
        Task<decimal> GetTotalAmountFilteredAsync(TransactionFilterModel transactionFilterModel);
        Task<bool> UpdateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto);
    }
}

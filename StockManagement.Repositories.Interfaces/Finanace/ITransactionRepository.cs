using StockManagement.Models.Dto.Finance;

namespace StockManagement.Repositories.Interfaces.Finanace
{
    public interface ITransactionRepository
    {
        Task<int> CreateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto);
        Task<bool> DeleteByDetailIdAsync(int currentUserId, int transactionDetailId);
        Task<List<TransactionDetailDto>> GetDetailByAccountTypeAsync(int accountTypeId);
        Task<bool> UpdateExpenseIncomeAsync(int currentUserId, TransactionDetailDto transactionDetailDto);

        //Task<int> CreateAsync(int currentUserId, TransactionDetailDto transactionDetailDto);
        //Task<bool> DeleteAsync(int currentUserId, int transactionDetailId);
        //Task<bool> UpdateAsync(int currentUserId, TransactionDetailDto transactionDetailDto);
    }
}

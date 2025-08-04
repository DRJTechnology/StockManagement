using StockManagement.Models.Finance;

namespace StockManagement.Services.Interfaces.Finance
{
    public interface ITransactionService
    {
        Task<int> CreateExpenseIncomeAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
        Task<bool> DeleteByDetailIdAsync(int currentUserId, int transactionDetailId);
        Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId);
        Task<bool> UpdateExpenseIncomeAsync(int currentUserId, TransactionDetailEditModel transactionDetail);

        //Task<int> CreateAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
        //Task<bool> DeleteAsync(int currentUserId, int transactionDetailId);
        //Task<List<TransactionDetailResponseModel>> GetAllAsync();
        //Task<bool> UpdateAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
    }
}

using StockManagement.Models.Finance;

namespace StockManagement.Services.Interfaces.Finance
{
    public interface ITransactionService
    {
        Task<int> CreateExpenseAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
        Task<bool> DeleteByDetailIdAsync(int currentUserId, int transactionDetailId);
        Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId);
        Task<bool> UpdateExpenseAsync(int currentUserId, TransactionDetailEditModel transactionDetail);

        //Task<int> CreateAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
        //Task<bool> DeleteAsync(int currentUserId, int transactionDetailId);
        //Task<List<TransactionDetailResponseModel>> GetAllAsync();
        //Task<bool> UpdateAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
    }
}

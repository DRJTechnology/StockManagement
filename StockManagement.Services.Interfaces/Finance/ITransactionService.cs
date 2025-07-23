using StockManagement.Models.Finance;

namespace StockManagement.Services.Interfaces.Finance
{
    public interface ITransactionService
    {
        //Task<int> CreateAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
        //Task<bool> DeleteAsync(int currentUserId, int transactionDetailId);
        //Task<List<TransactionDetailResponseModel>> GetAllAsync();
        //Task<bool> UpdateAsync(int currentUserId, TransactionDetailEditModel transactionDetail);
        Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId);
    }
}

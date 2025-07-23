using StockManagement.Models.Dto.Finance;

namespace StockManagement.Repositories.Interfaces.Finanace
{
    public interface ITransactionDetailRepository
    {
        //Task<int> CreateAsync(int currentUserId, TransactionDetailDto transactionDetailDto);
        //Task<bool> DeleteAsync(int currentUserId, int transactionDetailId);
        //Task<bool> UpdateAsync(int currentUserId, TransactionDetailDto transactionDetailDto);
        Task<List<TransactionDetailDto>> GetByAccountTypeAsync(int accountTypeId);
    }
}

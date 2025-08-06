using AutoMapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.Services.Finanace
{
    public class TransactionService(IMapper mapper, ITransactionRepository transactionRepository) : ITransactionService
    {
        public async Task<int> CreateExpenseIncomeAsync(int currentUserId, TransactionDetailEditModel transactionDetail)
        {
            var transactionDetailDto = mapper.Map<TransactionDetailDto>(transactionDetail);
            return await transactionRepository.CreateExpenseIncomeAsync(currentUserId, transactionDetailDto);
        }

        public async Task<bool> DeleteByDetailIdAsync(int currentUserId, int transactionDetailId)
        {
            return await transactionRepository.DeleteByDetailIdAsync(currentUserId, transactionDetailId);
        }

        public async Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId)
        {
            var accounts = mapper.Map<List<TransactionDetailResponseModel>>(await transactionRepository.GetDetailByAccountTypeAsync(accountTypeId));
            return accounts;
        }

        public async Task<TransactionFilteredResponseModel> GetFilteredAsync(TransactionFilterModel transactionFilterModel)
        {
            return await transactionRepository.GetFilteredAsync(transactionFilterModel);
        }

        public async Task<bool> UpdateExpenseIncomeAsync(int currentUserId, TransactionDetailEditModel transactionDetail)
        {
            var transactionDetailDto = mapper.Map<TransactionDetailDto>(transactionDetail);
            return await transactionRepository.UpdateExpenseIncomeAsync(currentUserId, transactionDetailDto);
        }
    }
}

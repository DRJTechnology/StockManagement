using AutoMapper;
using StockManagement.Models.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.Services.Finanace
{
    public class TransactionService(IMapper mapper, ITransactionDetailRepository transactionDetailRepository) : ITransactionService
    {
        public async Task<List<TransactionDetailResponseModel>> GetDetailByAccountTypeAsync(int accountTypeId)
        {
            var accounts = mapper.Map<List<TransactionDetailResponseModel>>(await transactionDetailRepository.GetByAccountTypeAsync(accountTypeId));
            return accounts;
        }
    }
}

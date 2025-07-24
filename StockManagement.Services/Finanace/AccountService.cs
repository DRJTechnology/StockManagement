using AutoMapper;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.Services.Finanace
{
    public class AccountService(IMapper mapper, IAccountRepository accountRepository) : IAccountService
    {
        public async Task<int> CreateAsync(int currentUserId, AccountEditModel account)
        {
            var accountDto = mapper.Map<AccountDto>(account);
            return await accountRepository.CreateAsync(currentUserId, accountDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int accountId)
        {
            return await accountRepository.DeleteAsync(currentUserId, accountId);
        }

        public async Task<List<AccountResponseModel>> GetAllAsync(bool includeInactive)
        {
            var accounts = mapper.Map<List<AccountResponseModel>>(await accountRepository.GetAllAsync(includeInactive));
            return accounts;
        }

        public async Task<List<AccountResponseModel>> GetByTypeAsync(int accountTypeId)
        {
            var accounts = mapper.Map<List<AccountResponseModel>>(await accountRepository.GetByTypeAsync(accountTypeId));
            return accounts;
        }

        public async Task<AccountResponseModel> GetByIdAsync(int accountId)
        {
            var account = await accountRepository.GetByIdAsync(accountId);
            return mapper.Map<AccountResponseModel>(account);
        }

        public async Task<bool> UpdateAsync(int currentUserId, AccountEditModel account)
        {
            var accountDto = mapper.Map<AccountDto>(account);
            return await accountRepository.UpdateAsync(currentUserId, accountDto);
        }
    }
}

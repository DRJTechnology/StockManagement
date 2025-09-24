using AutoMapper;
using StockManagement.Models.Finance;
using StockManagement.Repositories.Interfaces.Finanace;
using StockManagement.Services.Interfaces.Finance;

namespace StockManagement.Services.Finanace
{
    public class AccountTypeService(IMapper mapper, IAccountTypeRepository accountTypeRepository) : IAccountTypeService
    {
        public async Task<List<AccountTypeResponseModel>> GetAllAsync()
        {
            var productTypes = mapper.Map<List<AccountTypeResponseModel>>(await accountTypeRepository.GetAllAsync());
            return productTypes;
        }
    }
}

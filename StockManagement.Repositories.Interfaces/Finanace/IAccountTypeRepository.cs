using StockManagement.Models.Dto;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.Repositories.Interfaces.Finanace
{
    public interface IAccountTypeRepository
    {
        Task<List<AccountTypeDto>> GetAllAsync();
    }
}

using StockManagement.Client.Interfaces;
using StockManagement.Models.Finance;

namespace StockManagement.Client.Services
{
    public class AccountTypeDataService : GenericDataService<AccountTypeEditModel, AccountTypeResponseModel>, IAccountTypeDataService
    {
        public AccountTypeDataService(HttpClient httpClient)
            : base(httpClient)
        {
            ApiControllerName = "AccountType";
        }
    }
}

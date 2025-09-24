using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;

namespace StockManagement.Client.Services.Finance
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

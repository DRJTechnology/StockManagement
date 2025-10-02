using StockManagement.Client.Interfaces;

namespace StockManagement.ClientDataServices
{
    public class ClientGlobalSettingsService : IGlobalSettingsService
    {
        public string BusinessName => throw new NotImplementedException();

        public string BusinessWebsite => throw new NotImplementedException();

        public string BankAccountName => throw new NotImplementedException();

        public string BankAccountNumber => throw new NotImplementedException();

        public string BankAccountSortCode => throw new NotImplementedException();

        public string OwnerName => throw new NotImplementedException();

        public Task EnsureLoadedAsync()
        {
            throw new NotImplementedException();
        }
    }
}

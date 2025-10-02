namespace StockManagement.Client.Interfaces
{
    public interface IGlobalSettingsService
    {
        string BusinessName { get; }
        string BusinessWebsite { get; }
        string BankAccountName { get; }
        string BankAccountNumber { get; }
        string BankAccountSortCode { get; }
        string OwnerName { get; }

        Task EnsureLoadedAsync();
    }
}

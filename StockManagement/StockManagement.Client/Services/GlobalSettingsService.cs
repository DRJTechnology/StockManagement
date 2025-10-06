using StockManagement.Client.Interfaces;
using StockManagement.Models.Enums;

namespace StockManagement.Client.Services
{
    /// <summary>
    /// Provides global access to application settings with lazy, one-time load from the API.
    /// First access to any property triggers an asynchronous load of all settings.
    /// Consumers that need guaranteed values should await <see cref="EnsureLoadedAsync"/> before reading.
    /// </summary>
    public class GlobalSettingsService : IGlobalSettingsService
    {
        private readonly ISettingDataService settingDataService;
        private bool loaded;
        private Task? loadTask;
        private readonly object _lock = new();

        private string businessName = string.Empty;
        private string businessWebsite = string.Empty;
        private string bankAccountName = string.Empty;
        private string bankAccountNumber = string.Empty;
        private string bankAccountSortCode = string.Empty;
        private string ownerName = string.Empty;

        public GlobalSettingsService(ISettingDataService settingDataService)
        {
            this.settingDataService = settingDataService;
        }

        // Public properties (lazy trigger on first access)
        public string BusinessName { get { TriggerLoad(); return businessName; } }
        public string BusinessWebsite { get { TriggerLoad(); return businessWebsite; } }
        public string BankAccountName { get { TriggerLoad(); return bankAccountName; } }
        public string BankAccountNumber { get { TriggerLoad(); return bankAccountNumber; } }
        public string BankAccountSortCode { get { TriggerLoad(); return bankAccountSortCode; } }
        public string OwnerName { get { TriggerLoad(); return ownerName; } }

        /// <summary>
        /// Ensures the settings have been loaded (await this if you need values immediately).
        /// </summary>
        public Task EnsureLoadedAsync()
        {
            TriggerLoad();
            return loadTask ?? Task.CompletedTask;
        }

        private void TriggerLoad()
        {
            if (loaded || loadTask != null) return;
            lock (_lock)
            {
                if (loaded || loadTask != null) return;
                loadTask = LoadAsync();
            }
        }

        private async Task LoadAsync()
        {
            try
            {
                var settings = await settingDataService.GetAllAsync();
                foreach (var setting in settings)
                {
                    switch ((SettingEnum)setting.Id)
                    {
                        case SettingEnum.BusinessName:
                            businessName = setting.SettingValue;
                            break;
                        case SettingEnum.BusinessWebsite:
                            businessWebsite = setting.SettingValue;
                            break;
                        case SettingEnum.BankAccountName:
                            bankAccountName = setting.SettingValue;
                            break;
                        case SettingEnum.BankAccountNumber:
                            bankAccountNumber = setting.SettingValue;
                            break;
                        case SettingEnum.BankAccountSortCode:
                            bankAccountSortCode = setting.SettingValue;
                            break;
                        case SettingEnum.OwnerName:
                            ownerName = setting.SettingValue;
                            break;
                    }
                }
            }
            finally
            {
                loaded = true;
            }
        }
    }
}

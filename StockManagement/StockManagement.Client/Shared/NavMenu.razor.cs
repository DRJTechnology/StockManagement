using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Services;

namespace StockManagement.Client.Shared
{
    public partial class ClientNavMenuBase : ComponentBase, IDisposable
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        [Inject]
        protected ISettingDataService SettingService { get; set; } = default!;

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

        [Inject]
        protected IGlobalSettingsService GlobalSettings { get; set; } = default!;

        protected string? currentUrl;
        protected bool showSettingsSubMenu = false;
        protected bool showFinanceOptions = false;
        protected bool showSettingsOptions = false;
        protected bool mobileMenuActive = false;
        protected bool IsLoading { get; set; } = true;

        protected string BusinessName { get; set; } = "Stock Management";
        protected string OwnerAccountPageName { get; set; } = "Spreadsheet";

        protected override async Task OnInitializedAsync()
        {
            currentUrl = Navigation.ToBaseRelativePath(Navigation.Uri);
            Navigation.LocationChanged += OnLocationChanged;
            if (JSRuntime is IJSInProcessRuntime)
            {
                // Ensure global settings are loaded once and use them
                await GlobalSettings.EnsureLoadedAsync();
                var ownerName = GlobalSettings.OwnerName;
                BusinessName = string.IsNullOrWhiteSpace(GlobalSettings.BusinessName) ? BusinessName : GlobalSettings.BusinessName;
                OwnerAccountPageName = string.IsNullOrWhiteSpace(ownerName) ? "Spreadsheet" : $"{ownerName}'s Spreadsheet";
                IsLoading = false;
            }
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = Navigation.ToBaseRelativePath(e.Location);
            mobileMenuActive = false;
            StateHasChanged();
        }

        protected void ToggleFinanceOptions()
        {
            showFinanceOptions = !showFinanceOptions;
        }

        protected void ToggleSettingsOptions()
        {
            showSettingsOptions = !showSettingsOptions;
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}

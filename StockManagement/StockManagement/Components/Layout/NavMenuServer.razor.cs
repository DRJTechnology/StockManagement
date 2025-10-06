using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using StockManagement.Models;
using StockManagement.Models.Enums;
using StockManagement.Services.Interfaces;

namespace StockManagement.Components.Layout
{
    public partial class ServerNavMenuBase : ComponentBase, IDisposable
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        [Inject]
        protected ISettingService SettingService { get; set; } = default!;

        protected string? currentUrl;
        protected bool mobileMenuActive = false;
        protected string BusinessName { get; set; } = "Stock Management";
        protected List<SettingResponseModel> Settings { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            currentUrl = Navigation.ToBaseRelativePath(Navigation.Uri);
            Navigation.LocationChanged += OnLocationChanged;
            Settings = (await SettingService.GetAllAsync())?.ToList() ?? new();
            BusinessName = Settings.FirstOrDefault(s => s.Id == (int)SettingEnum.BusinessName)?.SettingValue ?? "Stock Management";
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = Navigation.ToBaseRelativePath(e.Location);
            mobileMenuActive = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}

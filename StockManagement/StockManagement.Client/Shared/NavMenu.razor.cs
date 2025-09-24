using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace StockManagement.Client.Shared
{
    public partial class ClientNavMenuBase : ComponentBase, IDisposable
    {
        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        protected string? currentUrl;
        protected bool showSettingsSubMenu = false;
        protected bool showFinanceOptions = false;
        protected bool showSettingsOptions = false;
        protected bool mobileMenuActive = false;

        protected override void OnInitialized()
        {
            currentUrl = Navigation.ToBaseRelativePath(Navigation.Uri);
            Navigation.LocationChanged += OnLocationChanged;
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

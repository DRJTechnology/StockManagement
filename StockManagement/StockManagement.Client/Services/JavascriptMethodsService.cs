using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;

namespace StockManagement.Client.Services
{
    public class JavascriptMethodsService : IJavascriptMethodsService
    {
        private readonly IJSRuntime JSRuntime;

        public JavascriptMethodsService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }
        public async Task<DateTime> GetLocalDateTimeAsync()
        {
            var isoString = await JSRuntime.InvokeAsync<string>("getLocalDateTime");
            // Parse as local time
            return DateTime.Parse(isoString);
        }
    }
}

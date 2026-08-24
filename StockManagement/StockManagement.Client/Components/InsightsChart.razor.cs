using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;

namespace StockManagement.Client.Components
{
    /// <summary>
    /// A Chart.js canvas. The chart is drawn after every render in which
    /// <see cref="Config"/> has changed, which is what makes the Insights
    /// page's dimension and measure toggles redraw without a data round trip.
    /// </summary>
    public partial class InsightsChartBase : ComponentBase, IAsyncDisposable
    {
        [Inject] protected IJavascriptMethodsService JavascriptMethods { get; set; } = default!;

        /// <summary>Canvas element id. Must be unique on the page.</summary>
        [Parameter, EditorRequired] public string Id { get; set; } = string.Empty;

        /// <summary>The Chart.js configuration object, serialised as-is.</summary>
        [Parameter, EditorRequired] public object? Config { get; set; }

        /// <summary>Chart height in pixels. The width always fills the container.</summary>
        [Parameter] public int Height { get; set; } = 300;

        private object? renderedConfig;
        private bool disposed;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Config == null || ReferenceEquals(Config, renderedConfig))
            {
                return;
            }

            renderedConfig = Config;
            await JavascriptMethods.RenderChartAsync(Id, Config);
        }

        public async ValueTask DisposeAsync()
        {
            if (disposed)
            {
                return;
            }
            disposed = true;

            try
            {
                await JavascriptMethods.DestroyChartAsync(Id);
            }
            catch (JSDisconnectedException)
            {
                // The circuit is already gone - nothing left to tear down.
            }
        }
    }
}

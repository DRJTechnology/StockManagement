namespace StockManagement.Client.Interfaces
{
    public interface IJavascriptMethodsService
    {
        Task<DateTime> GetLocalDateTimeAsync();

        /// <summary>
        /// Draws a Chart.js chart onto the canvas with the given id, replacing
        /// anything already on it. <paramref name="config"/> is serialised
        /// straight through as the Chart.js configuration object.
        /// </summary>
        Task RenderChartAsync(string canvasId, object config);

        /// <summary>Tears down the chart on the given canvas, if there is one.</summary>
        Task DestroyChartAsync(string canvasId);
    }
}

using StockManagement.Client.Interfaces;

namespace StockManagement.ClientDataServices
{
    public class ClientJavascriptMethodsService : IJavascriptMethodsService
    {
        public Task<DateTime> GetLocalDateTimeAsync()
        {
            throw new NotImplementedException();
        }

        public Task RenderChartAsync(string canvasId, object config)
        {
            throw new NotImplementedException();
        }

        public Task DestroyChartAsync(string canvasId)
        {
            throw new NotImplementedException();
        }
    }
}

using StockManagement.Models;

namespace StockManagement.Services.Interfaces
{
    public interface ILookupsService
    {
        Task<List<LookupsModel>> GetLookupsAsync();
    }
}

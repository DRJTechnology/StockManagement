using StockManagement.Models.Dto;

namespace StockManagement.Services.Interfaces
{
    public interface IErrorLogService
    {
        Task<int?> LogErrorAsync(ErrorLogDto errorDetails);
    }
}

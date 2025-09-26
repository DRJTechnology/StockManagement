using StockManagement.Models.Dto;

namespace StockManagement.Repositories.Interfaces
{
    public interface IErrorLogRepository
    {
        Task<int?> LogErrorAsync(ErrorLogDto errorDetails);
    }
}
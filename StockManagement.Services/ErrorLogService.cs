using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ErrorLogService(IErrorLogRepository errorLogRepository) : IErrorLogService
    {
        public async Task<int?> LogErrorAsync(ErrorLogDto errorDetails)
        {
            return await errorLogRepository.LogErrorAsync(errorDetails);
        }
    }
}

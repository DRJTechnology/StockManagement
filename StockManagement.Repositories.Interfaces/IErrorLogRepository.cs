namespace StockManagement.Repositories.Interfaces
{
    public interface IErrorLogRepository
    {
        Task<int?> LogErrorAsync(int? userId, string procedureName, int? errorNumber, int? errorSeverity, int? errorState, int? errorLine, string errorMessage);
        Task<int?> LogExceptionAsync(int userId, string procedureName, Exception exception);
    }
}
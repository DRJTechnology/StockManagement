using StockManagement.Models.Dto;
using StockManagement.Services.Interfaces;
using System.Diagnostics;

public class DatabaseLogger : ILogger
{
    private readonly string _categoryName;
    private readonly IServiceScopeFactory scopeFactory;

    public DatabaseLogger(string categoryName, IServiceScopeFactory scopeFactory)
    {
        _categoryName = categoryName;
        this.scopeFactory = scopeFactory;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Error;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        using var scope = scopeFactory.CreateScope();
        var errorLogService = scope.ServiceProvider.GetRequiredService<IErrorLogService>();
        var userContextAccessor = scope.ServiceProvider.GetRequiredService<IUserContextAccessor>();

        var message = formatter(state, exception);
        var stackTrace = exception?.StackTrace;
        var location = GetLocation();
        var userId = userContextAccessor.GetCurrentUserId();

        var errorDetails = new ErrorLogDto()
        {
            Location = location,
            LogLevel = logLevel.ToString(),
            ErrorMessage = $"{message}: {exception?.Message}",
            StackTrace = stackTrace,
            UserId = userId ?? 0
        };

        errorLogService.LogErrorAsync(errorDetails);
    }

    private string GetLocation()
    {
        var stack = new StackTrace(true);
        var frame = stack.GetFrame(2); // Adjust frame index as needed
        if (frame != null)
        {
            return $"{frame.GetFileName()}:{frame.GetFileLineNumber()}";
        }
        return _categoryName;
    }
}
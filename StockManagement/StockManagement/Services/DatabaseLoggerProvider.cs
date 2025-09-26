public class DatabaseLoggerProvider : ILoggerProvider
{
    private readonly IServiceScopeFactory scopeFactory;

    public DatabaseLoggerProvider(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new DatabaseLogger(categoryName, scopeFactory);
    }

    public void Dispose() { }
}
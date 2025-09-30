using System;
using System.Threading.Tasks;

public class ErrorNotificationService
{
    public event Func<string, Task>? OnError;

    public async Task NotifyErrorAsync(string message)
    {
        if (OnError != null)
            await OnError.Invoke(message);
    }
}
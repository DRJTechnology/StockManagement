using Microsoft.AspNetCore.Components;

namespace StockManagement.Client.Components
{
    public partial class ErrorDisplayBase : ComponentBase , IDisposable
    {
        [Inject]
        protected ErrorNotificationService ErrorService { get; set; } = null!;

        protected string? _message;
        protected bool _subscribed;

        protected override void OnInitialized()
        {
            if (!_subscribed)
            {
                ErrorService.OnError += HandleError;
                _subscribed = true;
            }
        }

        private Task HandleError(string msg)
        {
            _message = msg;
            StateHasChanged();
            return Task.CompletedTask;
        }

        protected void Clear()
        {
            _message = null;
            StateHasChanged();
        }

        public void Dispose()
        {
            if (_subscribed)
                ErrorService.OnError -= HandleError;
        }

    }
}

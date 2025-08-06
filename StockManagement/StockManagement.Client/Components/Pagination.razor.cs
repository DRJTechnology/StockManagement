using Microsoft.AspNetCore.Components;

namespace StockManagement.Client.Components
{
    public partial class PaginationBase : ComponentBase
    {
        [Parameter] public int CurrentPage { get; set; }
        [Parameter] public int TotalPages { get; set; }
        [Parameter] public EventCallback<int> OnPageChanged { get; set; }

        protected int StartPage { get; set; }
        protected int EndPage { get; set; }

        private int maxButtons = 5;

        protected override void OnParametersSet()
        {
            StartPage = Math.Max(1, CurrentPage - 2);
            EndPage = Math.Min(TotalPages, StartPage + maxButtons - 1);
            if (EndPage - StartPage < maxButtons - 1)
                StartPage = Math.Max(1, EndPage - maxButtons + 1);
        }

        protected async Task ChangePage(int page)
        {
            if (page < 1 || page > TotalPages || page == CurrentPage)
                return;
            await OnPageChanged.InvokeAsync(page);
        }
    }
}

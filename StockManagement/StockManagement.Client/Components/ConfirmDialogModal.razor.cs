using Microsoft.AspNetCore.Components;

public partial class ConfirmDialogModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }

    [Parameter] public EventCallback<bool> ConfirmationResult { get; set; }
    [Parameter] public string ItemName { get; set; } = string.Empty;

    protected string Title { get; set; } = "Confirm Delete";
    protected string Message { get; set; } = "Are you sure you want to delete this item?";

    protected override void OnParametersSet()
    {
        if (Show && !string.IsNullOrEmpty(ItemName))
        {
            Message = $"Are you sure you want to delete {ItemName}?";
        }
        else
        {
            Message = "Are you sure you want to delete this item?";
        }
    }

    protected async Task OnConfirmClicked()
    {
        Show = false;
        await ConfirmationResult.InvokeAsync(true);
    }

    protected async Task OnCancelClicked()
    {
        Show = false;
        await ConfirmationResult.InvokeAsync(false);
    }
}

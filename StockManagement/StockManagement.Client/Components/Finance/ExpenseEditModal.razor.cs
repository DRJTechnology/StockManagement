using Microsoft.AspNetCore.Components;
using StockManagement.Models.Finance;

public partial class ExpenseEditModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public TransactionDetailEditModel EditTransactionDetail { get; set; } = new();
    [Parameter] public List<AccountResponseModel> ExpenseAccounts { get; set; } = new();
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }
}

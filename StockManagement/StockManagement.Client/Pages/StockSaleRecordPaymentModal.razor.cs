using Microsoft.AspNetCore.Components;
using StockManagement.Models;

public partial class StockSaleRecordPaymentModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public StockSaleConfirmPaymentModel StockSaleConfirmPayment { get; set; } = new();
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnRecordPayment { get; set; }

    protected async Task HandleValidSubmit()
    {
        await OnRecordPayment.InvokeAsync();
    }
}

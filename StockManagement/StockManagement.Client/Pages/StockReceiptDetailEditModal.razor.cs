using Microsoft.AspNetCore.Components;
using StockManagement.Models;

public partial class StockReceiptDetailEditModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public StockReceiptDetailEditModel EditStockReceiptDetail { get; set; } = new();
    [Parameter] public LookupsModel Lookups { get; set; } = new();
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

[Authorize]
public partial class StockReceiptListBase : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IStockReceiptDataService StockReceiptService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<StockReceiptResponseModel> StockReceipts { get; set; } = new();
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadStockReceipts();
        }
    }

    protected async Task LoadStockReceipts()
    {
        StockReceipts = (await StockReceiptService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void AddStockReceipt()
    {
        Navigation.NavigateTo("/stock-receipt/0");
    }

    protected void ViewDetails(StockReceiptResponseModel stockReceipt)
    {
        Navigation.NavigateTo($"/stock-receipt/{stockReceipt.Id}");
    }

}

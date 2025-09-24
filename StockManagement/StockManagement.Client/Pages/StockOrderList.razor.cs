using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

[Authorize]
public partial class StockOrderListBase : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IStockOrderDataService StockOrderService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<StockOrderResponseModel> StockOrders { get; set; } = new();
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadStockOrders();
        }
    }

    protected async Task LoadStockOrders()
    {
        StockOrders = (await StockOrderService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void AddStockOrder()
    {
        Navigation.NavigateTo("/stock-order/0");
    }

    protected void ViewDetails(StockOrderResponseModel stockReceipt)
    {
        Navigation.NavigateTo($"/stock-order/{stockReceipt.Id}");
    }

}

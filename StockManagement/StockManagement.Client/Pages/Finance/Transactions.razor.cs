using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Interfaces.Finance;
using StockManagement.Client.Pages.Finance;
using StockManagement.Models;
using StockManagement.Models.Finance;

[Authorize]
public partial class TransactionsBase : ComponentBase
{
    [Inject]
    protected IAccountDataService AccountService { get; set; } = default!;

    [Inject]
    protected IContactDataService ContactService { get; set; } = default!;

    [Inject]
    protected ITransactionDataService TransactionService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<TransactionDetailResponseModel> TransactionDetails { get; set; } = new();
    protected List<AccountResponseModel> AccountList { get; set; } = new();

    protected bool IsLoading { get; set; }
    protected int TotalPages { get; set; }

    protected bool FiltersExpanded { get; set; } = false;

    protected TransactionFilterModel transactionFilterModel = new TransactionFilterModel();

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        // Only execute on the client (browser)
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadAccounts();
            await LoadTransactionDetailsAsync();
        }
    }

    protected async Task LoadTransactionDetailsAsync()
    {
        var transactionFilteredResponseModel = await TransactionService.GetFilteredAsync(transactionFilterModel);
        TransactionDetails = transactionFilteredResponseModel.TransactionDetailList;
        TotalPages = transactionFilteredResponseModel.TotalPages;
        IsLoading = false;
        await InvokeAsync(StateHasChanged);
    }
    protected async Task LoadAccounts()
    {
        AccountList = (await AccountService.GetAllAsync(false))?.ToList() ?? new();
    }

    protected void ToggleFilters()
    {
        FiltersExpanded = !FiltersExpanded;
    }

    protected async Task OnReset()
    {
        transactionFilterModel.FromDate = null;
        transactionFilterModel.ToDate = null;
        transactionFilterModel.AccountId = null;
        transactionFilterModel.TransactionTypeId = null;
        transactionFilterModel.CurrentPage = 1;
        await LoadTransactionDetailsAsync();
    }

    protected async Task OnFilter()
    {
        transactionFilterModel.CurrentPage = 1;
        await LoadTransactionDetailsAsync();
    }

    protected async Task GoToPage(int page)
    {
        if (page < 1 || page > TotalPages)
            return;
        transactionFilterModel.CurrentPage = page;
        await LoadTransactionDetailsAsync();
    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models;
using StockManagement.Models.Enums;
using StockManagement.Models.Finance;

[Authorize]
public partial class OwnersAccountBase : ComponentBase
{
    [Inject]
    protected IContactDataService ContactService { get; set; } = default!;

    [Inject]
    protected ITransactionDataService TransactionService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<TransactionDetailResponseModel> TransactionDetails { get; set; } = new();
    protected List<ContactResponseModel> ContactList { get; set; } = new();

    protected int TotalPages { get; set; }
    protected decimal TotalAmount { get; set; }
    protected bool IsLoading { get; set; }
    protected bool FiltersExpanded { get; set; } = false;

    const int OwnersAccountId = -1; // -1 is used by stored procedure to incude both Owner’s Capital/Investment and Drawings
    protected TransactionFilterModel transactionFilterModel = new TransactionFilterModel() { AccountId = OwnersAccountId };

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        // Only execute on the client (browser)
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadContacts();
            await LoadTransactionDetailsAsync();
        }
    }

    protected async Task LoadTransactionDetailsAsync()
    {
        var transactionFilteredResponseModel = await TransactionService.GetFilteredAsync(transactionFilterModel);
        TransactionDetails = transactionFilteredResponseModel.TransactionDetailList;
        TotalPages = transactionFilteredResponseModel.TotalPages;

        TotalAmount = await TransactionService.GetTotalAmountFilteredAsync(transactionFilterModel);
        TotalAmount *= -1;
        IsLoading = false;
        await InvokeAsync(StateHasChanged);
    }
    protected async Task LoadContacts()
    {
        ContactList = (await ContactService.GetAllAsync())?.ToList() ?? new();
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

    protected void ToggleFilters()
    {
        FiltersExpanded = !FiltersExpanded;
    }
    protected async Task GoToPage(int page)
    {
        if (page < 1 || page > TotalPages)
            return;
        transactionFilterModel.CurrentPage = page;
        await LoadTransactionDetailsAsync();
    }

}

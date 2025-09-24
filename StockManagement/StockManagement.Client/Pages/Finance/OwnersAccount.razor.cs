using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
    protected IAccountDataService AccountService { get; set; } = default!;

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


    protected List<AccountResponseModel> ExpenseAccounts { get; set; } = new();
    protected List<ContactResponseModel> Suppliers { get; set; } = new();
    protected List<AccountResponseModel> IncomeAccounts { get; set; } = new();
    protected List<ContactResponseModel> Customers { get; set; } = new();


    protected TransactionDetailEditModel EditTransactionDetail { get; set; } = new();
    protected bool ShowIncomeAddForm { get; set; }
    protected bool ShowExpenseAddForm { get; set; }

    const int OwnersAccountId = -1; // -1 is used by stored procedure to incude both Owner’s Capital/Investment and Drawings
    const int ExpenseAccountTypeId = 4; //  4 is the ID for Expense Account Type
    const int IncomeAccountTypeId = 3; //  3 is the ID for Revenue (Income) Account Type

    protected TransactionFilterModel transactionFilterModel = new TransactionFilterModel() { AccountId = OwnersAccountId };

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        // Only execute on the client (browser)
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadContacts();
            await LoadExpenseAccounts();
            await LoadIncomeAccounts();
            await LoadSuppliers();
            await LoadCustomers();
            await LoadTransactionDetailsAsync();
        }
    }

    protected async Task LoadTransactionDetailsAsync()
    {
        var transactionFilteredResponseModel = await TransactionService.GetFilteredAsync(transactionFilterModel);
        TransactionDetails = transactionFilteredResponseModel.TransactionDetailList;
        TotalPages = transactionFilteredResponseModel.TotalPages;

        await CalculateTotalAmount();
        IsLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task CalculateTotalAmount()
    {
        TotalAmount = await TransactionService.GetTotalAmountFilteredAsync(transactionFilterModel);
        TotalAmount *= -1;
    }

    protected async Task LoadContacts()
    {
        ContactList = (await ContactService.GetAllAsync())?.ToList() ?? new();
    }
    protected async Task LoadExpenseAccounts()
    {
        ExpenseAccounts = (await AccountService.GetByTypeAsync(ExpenseAccountTypeId))?.ToList() ?? new();
    }
    protected async Task LoadIncomeAccounts()
    {
        IncomeAccounts = (await AccountService.GetByTypeAsync(IncomeAccountTypeId))?.ToList() ?? new();
    }
    protected async Task LoadSuppliers()
    {
        Suppliers = (await ContactService.GetByTypeAsync((int)ContactTypeEnum.Supplier))?.ToList() ?? new();
    }
    protected async Task LoadCustomers()
    {
        Customers = (await ContactService.GetByTypeAsync((int)ContactTypeEnum.Customer))?.ToList() ?? new();
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
    protected void ShowAddIncomeForm(MouseEventArgs args)
    {
        EditTransactionDetail = new TransactionDetailEditModel() { AccountId = 0, Date = DateTime.Now, Direction = 1, TransactionType = TransactionTypeEnum.Income };
        ShowIncomeAddForm = true;
    }
    protected void ShowAddExpenseForm(MouseEventArgs args)
    {
        EditTransactionDetail = new TransactionDetailEditModel() { AccountId = 0, Date = DateTime.Now, Direction = -1, TransactionType = TransactionTypeEnum.Expense };
        ShowExpenseAddForm = true;
    }
    protected void CancelEdit()
    {
        ShowIncomeAddForm = false;
        ShowExpenseAddForm = false;
    }
    protected async Task HandleIncomeAddSubmit()
    {
        var newId = await TransactionService.CreateExpenseIncomeAsync(EditTransactionDetail);
        TransactionDetails.Insert(
            0,
            new TransactionDetailResponseModel()
            {
                Id = newId,
                //TransactionId = ?, TransactionId not required in front-end
                TransactionType = EditTransactionDetail.TransactionType,
                AccountId = EditTransactionDetail.AccountId,
                Date = EditTransactionDetail.Date,
                Description = EditTransactionDetail.Description,
                Amount = EditTransactionDetail.Amount,
                Direction = EditTransactionDetail.Direction,
                //Credit = EditTransactionDetail.Credit,
                //Debit = EditTransactionDetail.Debit,
                ContactId = EditTransactionDetail.ContactId,
                ContactName = Customers.Where(s => s.Id == EditTransactionDetail.ContactId).FirstOrDefault()?.Name ?? string.Empty,
            });
        await CalculateTotalAmount();
        ShowIncomeAddForm = false;
    }
    protected async Task HandleExpenseAddSubmit()
    {
        var newId = await TransactionService.CreateExpenseIncomeAsync(EditTransactionDetail);
        TransactionDetails.Insert(
            0,
            new TransactionDetailResponseModel()
            {
                Id = newId,
                //TransactionId = ?, TransactionId not required in front-end
                TransactionType = EditTransactionDetail.TransactionType,
                AccountId = EditTransactionDetail.AccountId,
                Date = EditTransactionDetail.Date,
                Description = EditTransactionDetail.Description,
                Amount = EditTransactionDetail.Amount,
                Direction = EditTransactionDetail.Direction,
                //Credit = EditTransactionDetail.Credit,
                //Debit = EditTransactionDetail.Debit,
                ContactId = EditTransactionDetail.ContactId,
                ContactName = Suppliers.Where(s => s.Id == EditTransactionDetail.ContactId).FirstOrDefault()!.Name,
            });
        await CalculateTotalAmount();
        ShowExpenseAddForm = false;
    }
}

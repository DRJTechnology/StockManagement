using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models;
using StockManagement.Models.Emuns;
using StockManagement.Models.Finance;

[Authorize]
public partial class ExpensesBase : ComponentBase
{
    [Inject]
    protected IAccountDataService AccountService { get; set; } = default!;

    [Inject]
    protected IContactDataService ContactService { get; set; } = default!;

    [Inject]
    protected ITransactionDataService TransactionService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<AccountResponseModel> ExpenseAccounts { get; set; } = new();
    protected List<ContactResponseModel> Suppliers { get; set; } = new();
    protected List<TransactionDetailResponseModel> TransactionDetails { get; set; } = new();
    protected TransactionDetailEditModel EditTransactionDetail { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;
    protected bool IsLoading { get; set; }

    const int ExpenseAccountTypeId = 4; //  4 is the ID for Expense Account Type

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        // Only execute on the client (browser)
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadExpenseAccounts();
            await LoadSuppliers();
            await LoadTransactionDetails();
        }
    }

    protected async Task LoadExpenseAccounts()
    {
        ExpenseAccounts = (await AccountService.GetByTypeAsync(ExpenseAccountTypeId))?.ToList() ?? new();
    }

    protected async Task LoadSuppliers()
    {
        Suppliers = (await ContactService.GetByTypeAsync((int)ContactTypeEnum.Supplier))?.ToList() ?? new();
    }
    protected async Task LoadTransactionDetails()
    {
        TransactionDetails = (await TransactionService.GetDetailByAccountTypeAsync(ExpenseAccountTypeId))?.ToList() ?? new();
        IsLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    protected void ShowAddForm()
    {
        EditTransactionDetail = new TransactionDetailEditModel() { AccountId = 0, Date = DateTime.Now };
        ShowForm = true;
    }

    protected void Edit(TransactionDetailResponseModel transactionDetail)
    {
        EditTransactionDetail = new TransactionDetailEditModel
        {
            Id = transactionDetail.Id,
            //TransactionId = ?, TransactionId not required in front-end
            AccountId = transactionDetail.AccountId,
            Date = transactionDetail.Date,
            Description = transactionDetail.Description,
            //Amount = transactionDetail.Amount,
            Amount = Math.Round(transactionDetail.Amount, 2),
            Direction = transactionDetail.Direction,
            Credit = transactionDetail.Credit,
            Debit = transactionDetail.Debit,
            ContactId = transactionDetail.ContactId,
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditTransactionDetail.Id == 0)
        {
            var newId = await TransactionService.CreateExpenseAsync(EditTransactionDetail);
            TransactionDetails.Add(
                new TransactionDetailResponseModel()
                {
                    Id = newId,
                    //TransactionId = ?, TransactionId not required in front-end
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
        }
        else
        {
            await TransactionService.UpdateExpenseAsync(EditTransactionDetail);
            var index = TransactionDetails.FindIndex(p => p.Id == EditTransactionDetail.Id);
            if (index >= 0)
            {
                TransactionDetails[index] = new TransactionDetailResponseModel()
                {
                    Id = EditTransactionDetail.Id,
                    //TransactionId = ?, TransactionId not required in front-end
                    AccountId = EditTransactionDetail.AccountId,
                    Date = EditTransactionDetail.Date,
                    Description = EditTransactionDetail.Description,
                    Amount = EditTransactionDetail.Amount,
                    Direction = EditTransactionDetail.Direction,
                    //Credit = EditTransactionDetail.Credit,
                    //Debit = EditTransactionDetail.Debit,
                    ContactId = EditTransactionDetail.ContactId,
                    ContactName = Suppliers.Where(s => s.Id == EditTransactionDetail.ContactId).FirstOrDefault()!.Name,
                };
            }
        }
        ShowForm = false;
    }

    protected void CancelEdit()
    {
        ShowForm = false;
    }

    protected void Delete(int id)
    {
        EditTransactionDetail = new TransactionDetailEditModel { Id = id };
        SelectedItemName = TransactionDetails.Where(p => p.Id == EditTransactionDetail.Id).FirstOrDefault()?.Description ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        if (confirmed)
        {
            await TransactionService.DeleteByDetailIdAsync(EditTransactionDetail.Id);
            TransactionDetails.RemoveAll(p => p.Id == EditTransactionDetail.Id);
            EditTransactionDetail = new TransactionDetailEditModel();
        }
    }
}

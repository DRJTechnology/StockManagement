using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;

[Authorize]
public partial class ExpensesBase : ComponentBase
{
    [Inject]
    protected IAccountDataService AccountService { get; set; } = default!;

    [Inject]
    protected ITransactionDataService TransactionService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<AccountResponseModel> ExpenseAccounts { get; set; } = new();
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
            await LoadTransactionDetails();
        }
    }

    protected async Task LoadExpenseAccounts()
    {
        ExpenseAccounts = (await AccountService.GetByTypeAsync(ExpenseAccountTypeId))?.ToList() ?? new();
        IsLoading = false;
    }
    protected async Task LoadTransactionDetails()
    {
        TransactionDetails = (await TransactionService.GetDetailByAccountTypeAsync(ExpenseAccountTypeId))?.ToList() ?? new();
        IsLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    protected void ShowAddForm()
    {
        EditTransactionDetail = new TransactionDetailEditModel();
        ShowForm = true;
    }

    protected void Edit(TransactionDetailResponseModel transactionDetail)
    {
        EditTransactionDetail = new TransactionDetailEditModel
        {
            Id = transactionDetail.Id,
            //Name = transactionDetail.Name,
            //Notes = transactionDetail.Notes,
            //ExpenseTypeId = transactionDetail.ExpenseTypeId,
            //Active = transactionDetail.Active,
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        //if (EditTransactionDetail.Id == 0)
        //{
        //    var newId = await ExpenseService.CreateAsync(EditTransactionDetail);
        //    TransactionDetails.Add(
        //        new ExpenseResponseModel()
        //            { 
        //            Id = newId, 
        //            Name = EditTransactionDetail.Name,
        //            Notes = EditTransactionDetail.Notes,
        //            ExpenseTypeId = EditTransactionDetail.ExpenseTypeId,
        //            Type = ExpenseTypes.FirstOrDefault(at => at.Id == EditTransactionDetail.ExpenseTypeId)!.Type,
        //            Active = EditTransactionDetail.Active,
        //            Deleted = false 
        //        });
        //}
        //else
        //{
        //    await ExpenseService.UpdateAsync(EditTransactionDetail);
        //    var index = TransactionDetails.FindIndex(p => p.Id == EditTransactionDetail.Id);
        //    if (index >= 0)
        //    {
        //        TransactionDetails[index] = new ExpenseResponseModel()
        //        {
        //            Id = EditTransactionDetail.Id,
        //            Name = EditTransactionDetail.Name,
        //            Notes = EditTransactionDetail.Notes,
        //            ExpenseTypeId = EditTransactionDetail.ExpenseTypeId,
        //            Type = ExpenseTypes.FirstOrDefault(at => at.Id == EditTransactionDetail.ExpenseTypeId)!.Type,
        //            Active = EditTransactionDetail.Active,
        //            Deleted = false
        //        };
        //    }
        //}
        ShowForm = false;
    }

    protected void CancelEdit()
    {
        ShowForm = false;
    }

    protected void Delete(int id)
    {
        //EditTransactionDetail = new ExpenseEditModel { Id = id };
        //SelectedItemName = TransactionDetails.Where(p => p.Id == EditTransactionDetail.Id).FirstOrDefault()?.Name ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        //if (confirmed)
        //{
        //    await ExpenseService.DeleteAsync(EditTransactionDetail.Id);
        //    TransactionDetails.RemoveAll(p => p.Id == EditTransactionDetail.Id);
        //    EditTransactionDetail = new ExpenseEditModel();
        //}
    }
}

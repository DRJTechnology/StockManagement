using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces.Finance;
using StockManagement.Models.Finance;

[Authorize]
public partial class AccountsBase : ComponentBase
{
    [Inject]
    protected IAccountTypeDataService AccountTypeService { get; set; } = default!;

    [Inject]
    protected IAccountDataService AccountService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<AccountTypeResponseModel> AccountTypes { get; set; } = new();
    protected List<AccountResponseModel> Accounts { get; set; } = new();
    protected AccountEditModel EditAccount { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;
    protected bool IsLoading { get; set; }

    private bool _includeInactive = false;
    protected bool IncludeInactive
    {
        get => _includeInactive;
        set
        {
            if (_includeInactive != value)
            {
                _includeInactive = value;
                LoadAccounts();
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        // Only execute on the client (browser)
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadAccountTypes();
            await LoadAccounts();
        }
    }

    protected async Task LoadAccountTypes()
    {
        AccountTypes = (await AccountTypeService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }
    protected async Task LoadAccounts()
    {
        Accounts = (await AccountService.GetAllAsync(_includeInactive))?.ToList() ?? new();
        IsLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    protected void ShowAddForm()
    {
        EditAccount = new AccountEditModel();
        ShowForm = true;
    }

    protected void Edit(AccountResponseModel account)
    {
        EditAccount = new AccountEditModel
        {
            Id = account.Id,
            Name = account.Name,
            Notes = account.Notes,
            AccountTypeId = account.AccountTypeId,
            Active = account.Active,
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditAccount.Id == 0)
        {
            var newId = await AccountService.CreateAsync(EditAccount);
            Accounts.Add(
                new AccountResponseModel()
                    { 
                    Id = newId, 
                    Name = EditAccount.Name,
                    Notes = EditAccount.Notes,
                    AccountTypeId = EditAccount.AccountTypeId,
                    Type = AccountTypes.FirstOrDefault(at => at.Id == EditAccount.AccountTypeId)!.Type,
                    Active = EditAccount.Active,
                    Deleted = false 
                });
        }
        else
        {
            await AccountService.UpdateAsync(EditAccount);
            var index = Accounts.FindIndex(p => p.Id == EditAccount.Id);
            if (index >= 0)
            {
                Accounts[index] = new AccountResponseModel()
                {
                    Id = EditAccount.Id,
                    Name = EditAccount.Name,
                    Notes = EditAccount.Notes,
                    AccountTypeId = EditAccount.AccountTypeId,
                    Type = AccountTypes.FirstOrDefault(at => at.Id == EditAccount.AccountTypeId)!.Type,
                    Active = EditAccount.Active,
                    Deleted = false
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
        EditAccount = new AccountEditModel { Id = id };
        SelectedItemName = Accounts.Where(p => p.Id == EditAccount.Id).FirstOrDefault()?.Name ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        if (confirmed)
        {
            await AccountService.DeleteAsync(EditAccount.Id);
            Accounts.RemoveAll(p => p.Id == EditAccount.Id);
            EditAccount = new AccountEditModel();
        }
    }
}

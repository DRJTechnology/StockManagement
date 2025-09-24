using Microsoft.AspNetCore.Components;
using StockManagement.Models;
using StockManagement.Models.Enums;
using StockManagement.Models.Finance;

public partial class TransactionDetailEditModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public TransactionDetailEditModel EditTransactionDetail { get; set; } = new();
    [Parameter] public List<AccountResponseModel> Accounts { get; set; } = new();
    [Parameter] public List<ContactResponseModel> Contacts { get; set; } = new();
    [Parameter] public TransactionTypeEnum TransactionType { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }

    protected string ContactType { get; set; } = string.Empty;

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }
    protected override void OnParametersSet()
    {
        if (Show)
        {
            switch (TransactionType)
            {
                case TransactionTypeEnum.Journal:
                    break;
                case TransactionTypeEnum.Expense:
                    ContactType = "Supplier";
                    break;
                case TransactionTypeEnum.Income:
                    ContactType = "Customer";
                    break;
                case TransactionTypeEnum.Sales:
                    ContactType = "Supplier/Customer";
                    break;
                default:
                    break;
            }
        }
    }

}

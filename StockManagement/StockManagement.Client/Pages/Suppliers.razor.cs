using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

[Authorize]
public partial class SuppliersBase : ComponentBase
{
    [Inject]
    protected ISupplierDataService SupplierService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<SupplierResponseModel> Suppliers { get; set; } = new();
    protected SupplierEditModel EditSupplier { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadSuppliers();
        }
    }

    protected async Task LoadSuppliers()
    {
        Suppliers = (await SupplierService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void ShowAddForm()
    {
        EditSupplier = new SupplierEditModel();
        ShowForm = true;
    }

    protected void Edit(SupplierResponseModel Supplier)
    {
        EditSupplier = new SupplierEditModel
        {
            Id = Supplier.Id,
            SupplierName = Supplier.SupplierName
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditSupplier.Id == 0)
        {
            var newId = await SupplierService.CreateAsync(EditSupplier);
            Suppliers.Add(
                new SupplierResponseModel()
                {
                    Id = newId,
                    SupplierName = EditSupplier.SupplierName,
                    Deleted = false
                });
        }
        else
        {
            await SupplierService.UpdateAsync(EditSupplier);
            var index = Suppliers.FindIndex(pt => pt.Id == EditSupplier.Id);
            if (index >= 0)
            {
                Suppliers[index] = new SupplierResponseModel()
                {
                    Id = EditSupplier.Id,
                    SupplierName = EditSupplier.SupplierName,
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

    protected async Task Delete(int id)
    {
        await SupplierService.DeleteAsync(id);
        Suppliers.RemoveAll(pt => pt.Id == id);
    }
}

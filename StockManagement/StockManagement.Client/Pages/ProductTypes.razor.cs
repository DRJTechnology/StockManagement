using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Pages;
using StockManagement.Models;

[Authorize]
public partial class ProductTypesBase : ComponentBase
{
    [Inject]
    protected IProductTypeDataService ProductTypeService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<ProductTypeResponseModel> ProductTypes { get; set; } = new();
    protected ProductTypeEditModel EditProductType { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadProductTypes();
        }
    }

    protected async Task LoadProductTypes()
    {
        ProductTypes = (await ProductTypeService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void ShowAddForm()
    {
        EditProductType = new ProductTypeEditModel();
        ShowForm = true;
    }

    protected void Edit(ProductTypeResponseModel productType)
    {
        EditProductType = new ProductTypeEditModel
        {
            Id = productType.Id,
            ProductTypeName = productType.ProductTypeName
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditProductType.Id == 0)
        {
            var newId = await ProductTypeService.CreateAsync(EditProductType);
            ProductTypes.Add(
                new ProductTypeResponseModel()
                {
                    Id = newId,
                    ProductTypeName = EditProductType.ProductTypeName,
                    Deleted = false
                });
        }
        else
        {
            await ProductTypeService.UpdateAsync(EditProductType);
            var index = ProductTypes.FindIndex(pt => pt.Id == EditProductType.Id);
            if (index >= 0)
            {
                ProductTypes[index] = new ProductTypeResponseModel()
                {
                    Id = EditProductType.Id,
                    ProductTypeName = EditProductType.ProductTypeName,
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
        EditProductType = new ProductTypeEditModel { Id = id };
        SelectedItemName = ProductTypes.Where(p => p.Id == EditProductType.Id).FirstOrDefault()?.ProductTypeName ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        if (confirmed)
        {
            await ProductTypeService.DeleteAsync(EditProductType.Id);
            ProductTypes.RemoveAll(p => p.Id == EditProductType.Id);
            EditProductType = new ProductTypeEditModel();
        }
    }

}

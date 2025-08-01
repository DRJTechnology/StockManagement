using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

[Authorize]
public partial class ProductsBase : ComponentBase
{
    [Inject]
    protected IProductDataService ProductService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<ProductResponseModel> Products { get; set; } = new();
    protected ProductEditModel EditProduct { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected string SelectedItemName { get; set; } = string.Empty;
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        // Only execute on the client (browser)
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadProducts();
        }
    }

    protected async Task LoadProducts()
    {
        Products = (await ProductService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void ShowAddForm()
    {
        EditProduct = new ProductEditModel();
        ShowForm = true;
    }

    protected void Edit(ProductResponseModel product)
    {
        EditProduct = new ProductEditModel
        {
            Id = product.Id,
            ProductName = product.ProductName
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditProduct.Id == 0)
        {
            var newId = await ProductService.CreateAsync(EditProduct);
            Products.Add(
                new ProductResponseModel()
                    { 
                    Id = newId, 
                    ProductName = EditProduct.ProductName, 
                    Deleted = false 
                });
        }
        else
        {
            await ProductService.UpdateAsync(EditProduct);
            var index = Products.FindIndex(p => p.Id == EditProduct.Id);
            if (index >= 0)
            {
                Products[index] = new ProductResponseModel()
                                    {
                                        Id = EditProduct.Id,
                                        ProductName = EditProduct.ProductName,
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
        EditProduct = new ProductEditModel { Id = id };
        SelectedItemName = Products.Where(p => p.Id == EditProduct.Id).FirstOrDefault()?.ProductName ?? string.Empty;
        ShowDeleteConfirm = true;
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        ShowDeleteConfirm = false;
        if (confirmed)
        {
            await ProductService.DeleteAsync(EditProduct.Id);
            Products.RemoveAll(p => p.Id == EditProduct.Id);
            EditProduct = new ProductEditModel();
        }
    }
}

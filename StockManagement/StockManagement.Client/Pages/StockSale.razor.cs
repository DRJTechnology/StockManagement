using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Services;
using StockManagement.Models;
using StockManagement.Models.Enums;

public partial class StockSaleBase : ComponentBase
{
    [Parameter] public int StockSaleId { get; set; }

    [Inject]
    IJSRuntime JS { get; set; }

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IStockSaleDataService StockSaleService { get; set; } = default!;

    [Inject]
    protected IStockSaleDetailDataService StockSaleDetailService { get; set; } = default!;

    [Inject]
    protected IInventoryBatchDataService InventoryBatchService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    protected IContactDataService ContactService { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected LookupsModel Lookups { get; private set; }
    protected bool ShowDeleteNoteConfirm { get; set; } = false;
    protected bool ShowDeleteDetailConfirm { get; set; } = false;
    protected bool ShowSaleConfirmForm { get; set; } = false;
    protected bool ShowSaleConfirmPayment { get; set; } = false;
    protected bool ShowResetConfirm { get; set; } = false;

    protected string SelectedItemName { get; set; } = string.Empty;

    protected bool IsLoading { get; set; }
    protected StockSaleResponseModel EditStockSale { get; set; } = new();
    protected StockSaleDetailEditModel EditStockSaleDetail { get; set; } = new();
    protected StockSaleConfirmationModel StockSaleConfirmationObject { get; set; } = new();
    protected StockSaleConfirmPaymentModel StockSaleConfirmPayment { get; set; } = new();
    protected List<ContactResponseModel> Customers { get; set; } = new();

    protected bool ShowEditDetailForm { get; set; }
    protected decimal StockCostPrice { get; set; } = 0;

    protected EditContext editContext;
    protected bool IsFormValid = false;
    protected bool IsDirty = false;
    private bool _postInitActionDone = false;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadLookups();
            await LoadCustomers();
            await LoadStockSale();
        }
        // Initialization done, trigger re-render
        StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Only run after initialization and only once
        if (!_postInitActionDone && editContext != null)
        {
            IsFormValid = editContext.Validate();
            _postInitActionDone = true;
            StateHasChanged();
        }
    }
    protected async Task HandleValidSubmit()
    {
        if (EditStockSale.Id == 0)
        {
            var newId = await StockSaleService.CreateAsync(EditStockSale);
            EditStockSale.Id = newId;
        }
        else
        {
            await StockSaleService.UpdateAsync(EditStockSale);
        }
        IsDirty = false;
    }

    protected async Task LoadStockSale()
    {
        if (StockSaleId == 0)
        {
            var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
            EditStockSale = new StockSaleResponseModel() { Date = localNow, LocationId = 0, ContactId = 0, DetailList = new List<StockSaleDetailResponseModel>() };
        }
        else
        {
            EditStockSale = await StockSaleService.GetByIdAsync(StockSaleId);
            if (EditStockSale == null)
            {
                throw new Exception("Invalid Stock Sale");
            }
        }
        CreateValidationContext();

        if (EditStockSale.SaleConfirmed)
        {
            StockCostPrice = await InventoryBatchService.GetSaleCostAsync(EditStockSale.Id);
        }

        IsLoading = false;
    }
    protected async Task LoadCustomers()
    {
        Customers = (await ContactService.GetByTypeAsync((int)ContactTypeEnum.Customer))?.ToList() ?? new();
    }

    private void CreateValidationContext()
    {
        editContext = new EditContext(EditStockSale);
        IsFormValid = editContext.Validate();
        editContext.OnFieldChanged += (_, __) =>
        {
            IsFormValid = editContext.Validate();
            IsDirty = true;
            InvokeAsync(StateHasChanged);
        };
    }

    private async Task LoadLookups()
    {
        var lookupsList = await LookupsService.GetAllAsync();
        Lookups = lookupsList.FirstOrDefault();
    }

    protected async Task ShowAddDetailForm()
    {
        var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
        EditStockSaleDetail = new StockSaleDetailEditModel() { StockSaleId = EditStockSale.Id };
        ShowEditDetailForm = true;
    }

    protected void CancelDetailEdit()
    {
        ShowEditDetailForm = false;
    }
    protected async Task HandleValidDetailSubmit()
    {
        if (EditStockSaleDetail.Id == 0)
        {
            var newId = await StockSaleDetailService.CreateAsync(EditStockSaleDetail);
            EditStockSale.DetailList.Add(
                new StockSaleDetailResponseModel()
                {
                    Id = newId,
                    StockSaleId = EditStockSale.Id,
                    ProductId = EditStockSaleDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditStockSaleDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditStockSaleDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditStockSaleDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditStockSaleDetail.Quantity,
                    Deleted = false
                });
        }
        else
        {
            await StockSaleDetailService.UpdateAsync(EditStockSaleDetail);
            var index = EditStockSale.DetailList.FindIndex(dnd => dnd.Id == EditStockSaleDetail.Id);
            if (index >= 0)
            {
                EditStockSale.DetailList[index] = new StockSaleDetailResponseModel()
                {
                    Id = EditStockSaleDetail.Id,
                    StockSaleId = EditStockSale.Id,
                    ProductId = EditStockSaleDetail.ProductId,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditStockSaleDetail.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeId = EditStockSaleDetail.ProductTypeId,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditStockSaleDetail.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    Quantity = EditStockSaleDetail.Quantity,
                    Deleted = false
                };
            }
        }
        ShowEditDetailForm = false;
    }

    protected void DeleteStockSale()
    {
        if (EditStockSale.Id > 0)
        {
            SelectedItemName = "this entire Delivery Note";
            ShowDeleteNoteConfirm = true;
        }
    }

    protected async Task HandleDeleteConfirmation(bool confirmed)
    {
        if (confirmed && ShowDeleteNoteConfirm)
        {
            if (confirmed)
            {
                await StockSaleService.DeleteAsync(EditStockSale.Id);
                Navigation.NavigateTo("/stock-sale-list");
            }
        }
        else if (confirmed && ShowDeleteDetailConfirm)
        {
            await StockSaleDetailService.DeleteAsync(EditStockSaleDetail.Id);
            EditStockSale.DetailList.RemoveAll(dnd => dnd.Id == EditStockSaleDetail.Id);
        }

        ShowDeleteNoteConfirm = false;
        ShowDeleteDetailConfirm = false;
    }

    protected void ResetStockSale()
    {
        if (EditStockSale.Id > 0)
        {
            //SelectedItemName = "this entire Delivery Note";
            ShowResetConfirm = true;
        }
    }

    protected async Task HandleResetConfirmation(bool confirmed)
    {
        if (confirmed)
        {
            await StockSaleService.ResetAsync(EditStockSale.Id);
            EditStockSale.SaleConfirmed = false;
            EditStockSale.PaymentReceived = false;
            EditStockSale.TotalPrice = 0;
            StockCostPrice = 0;
        }

        ShowResetConfirm = false;
    }

    protected void EditDetail(StockSaleDetailResponseModel activity)
    {
        EditStockSaleDetail = new StockSaleDetailEditModel
        {
            Id = activity.Id,
            StockSaleId = EditStockSale.Id,
            ProductId = activity.ProductId,
            ProductTypeId = activity.ProductTypeId,
            Quantity = activity.Quantity,
            Deleted = activity.Deleted
        };
        ShowEditDetailForm = true;
    }

    protected void DeleteDetail(int id)
    {
        EditStockSaleDetail = new StockSaleDetailEditModel { Id = id };
        SelectedItemName = EditStockSale.DetailList.Where(p => p.Id == id).FirstOrDefault()?.ProductName ?? string.Empty;
        ShowDeleteDetailConfirm = true;
    }

    protected async Task DownloadInvoicePdf()
    {
        var url = $"/api/pdf/invoice/{EditStockSale.Id}";
        await JS.InvokeVoidAsync("open", url, "_blank");
    }
    public int LocationId
    {
        get => EditStockSale.LocationId;
        set
        {
            if (EditStockSale.LocationId != value)
            {
                EditStockSale.LocationId = value;
                var location = Lookups.LocationList.Where(l => l.Id == EditStockSale.LocationId).FirstOrDefault();
                EditStockSale.ContactId = location != null ? location.ContactId : default;
            }
        }
    }

    protected void CancelConfirmSale()
    {
        ShowSaleConfirmForm = false;
    }

    protected async Task HandleConfirmSale()
    {
        var response = await StockSaleService.ConfirmStockSale(StockSaleConfirmationObject);
        if (response)
        {
            EditStockSale.SaleConfirmed = true;
            EditStockSale.TotalPrice = StockSaleConfirmationObject.TotalPrice;
            StockCostPrice = await InventoryBatchService.GetSaleCostAsync(EditStockSale.Id);
        }
        ShowSaleConfirmForm = false;
    }

    protected async Task ConfirmSale()
    {
        StockSaleConfirmationObject = new StockSaleConfirmationModel()
        {
            StockSaleId = EditStockSale.Id,
            LocationId = EditStockSale.LocationId,
            ContactId = EditStockSale.ContactId,
            StockSaleDetails = EditStockSale.DetailList
            .Select(detail => new StockSaleDetailResponseModel
            {
                Id = detail.Id,
                StockSaleId = detail.StockSaleId,
                ProductId = detail.ProductId,
                ProductName = detail.ProductName,
                ProductTypeId = detail.ProductTypeId,
                ProductTypeName = detail.ProductTypeName,
                Quantity = detail.Quantity,
                Deleted = detail.Deleted,
                UnitPrice = Math.Round(Lookups.ProductTypeList.FirstOrDefault(pt => pt.Id == detail.ProductTypeId)?.DefaultSalePrice ?? 0, 2),
            })
            .ToList(),
        };

        ShowSaleConfirmForm = true;
    }

    protected void RecordPayment()
    {
        StockSaleConfirmPayment = new StockSaleConfirmPaymentModel()
        {
            StockSaleId = EditStockSale.Id,
            PaymentDate = DateTime.Now.Date,
            Description = DescribeSale(),
        };
        ShowSaleConfirmPayment = true;
    }

    private string DescribeSale()
    {
        var saleDescription = $"Sale to {Customers.FirstOrDefault(c => c.Id == EditStockSale.ContactId)?.Name} of ";
        var productTypeQuantities = EditStockSale.DetailList
            .GroupBy(d => d.ProductTypeName)
            .Select(g => new
            {
                ProductTypeName = g.Key,
                TotalQuantity = g.Sum(d => d.Quantity)
            })
            .ToList();

        foreach (var ptq in productTypeQuantities)
        {
            saleDescription += $"{ptq.TotalQuantity} x {ptq.ProductTypeName}, ";
        }
        saleDescription = saleDescription.TrimEnd(',', ' ');
        return saleDescription;
    }

    protected async Task HandleRecordPayment()
    {
        var response = await StockSaleService.ConfirmStockSalePayment(StockSaleConfirmPayment);
        if (response)
        {
            EditStockSale.PaymentReceived = true;
        }
        ShowSaleConfirmPayment = false;
    }

    protected void CancelRecordPaymentSale()
    {
        ShowSaleConfirmPayment = false;
    }
}

using Microsoft.AspNetCore.Components;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Reports;

public partial class DeliveryNoteDetailEditModalBase : ComponentBase
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public DeliveryNoteDetailEditModel EditDeliveryNoteDetail { get; set; } = new();
    [Parameter] public LookupsModel Lookups { get; set; } = new();
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnValidSubmit { get; set; }

    [Inject]
    protected IReportDataService ReportDataService { get; set; } = default!;

    protected List<StockReportItemDto> StockReportItems = new();
    protected int availableQuantity = 0;

    public int ProductTypeId
    {
        get => EditDeliveryNoteDetail.ProductTypeId;
        set
        {
            Console.WriteLine("ProductTypeId setter");
            if (EditDeliveryNoteDetail.ProductTypeId != value)
            {
                EditDeliveryNoteDetail.ProductTypeId = value;
                _ = GetAvailableQuantity();
            }
        }
    }

    public int ProductId
    {
        get => EditDeliveryNoteDetail.ProductId;
        set
        {
            Console.WriteLine("ProductId setter");
            if (EditDeliveryNoteDetail.ProductId != value)
            {
                EditDeliveryNoteDetail.ProductId = value;
                _ = GetAvailableQuantity();
            }
        }
    }

    protected async Task HandleValidSubmit()
    {
        await OnValidSubmit.InvokeAsync();
    }

    protected async Task GetAvailableQuantity()
    {
        if (EditDeliveryNoteDetail.ProductTypeId > 0 && EditDeliveryNoteDetail.ProductId > 0)
        {
            StockReportItems = await ReportDataService.GetStockReportAsync(1, // Stock room
                                                                            EditDeliveryNoteDetail.ProductTypeId,
                                                                            EditDeliveryNoteDetail.ProductId);
            if (StockReportItems.Count > 0)
            {
                availableQuantity = StockReportItems.FirstOrDefault()!.ActiveQuantity;      
            }
            else
            {
                availableQuantity = 0;
            }
        }
        else
        {
            availableQuantity = 0;
        }
        await InvokeAsync(StateHasChanged);
    }

    protected async Task Close()
    {
        availableQuantity = 0;
        await OnCancel.InvokeAsync();
    }
}

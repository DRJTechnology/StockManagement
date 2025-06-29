using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Services;
using StockManagement.Models;

public partial class DeliveryNoteBase : ComponentBase
{
    [Parameter] public int DeliveryNoteId { get; set; }

    [Inject]
    protected IDeliveryNoteDataService DeliveryNoteService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected LookupsModel Lookups { get; private set; }

    protected bool IsLoading { get; set; }
    protected DeliveryNoteEditModel EditDeliveryNote { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadLookups();
            await LoadDeliveryNote();
        }
    }

    protected async Task HandleValidSubmit()
    {
    }

    protected async Task LoadDeliveryNote()
    {
        if (DeliveryNoteId == 0) {
            var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
            EditDeliveryNote = new DeliveryNoteEditModel() { Date = localNow };
        } else
        {
            EditDeliveryNote = await DeliveryNoteService.GetByIdAsync(DeliveryNoteId);
            if (EditDeliveryNote == null)
            {
                EditDeliveryNote = new DeliveryNoteEditModel();
            }
        }
        IsLoading = false;
    }

    private async Task LoadLookups()
    {
        var lookupsList = await LookupsService.GetAllAsync();
        Lookups = lookupsList.FirstOrDefault();
    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using System.Diagnostics;

[Authorize]
public partial class ActivitiesBase : ComponentBase
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    [Inject]
    protected IActivityDataService ActivityService { get; set; } = default!;

    [Inject]
    protected ILookupsDataService LookupsService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    protected IJavascriptMethodsService JavascriptMethodsService { get; set; } = default!;

    protected List<ActivityResponseModel> Activities { get; set; } = new();
    protected ActivityEditModel EditActivity { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool ShowDeleteConfirm { get; set; } = false;
    protected bool IsLoading { get; set; }
    protected bool filtersExpanded = false;

    public LookupsModel Lookups { get; private set; }

    protected ActivityFilterModel activityFilterModel = new ActivityFilterModel();

    protected int TotalPages { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadActivities();
            await LoadLookups();
            IsLoading = false;
        }
        System.Diagnostics.Debug.WriteLine("Loaded!");

    }

    protected async Task LoadActivities()
    {
        var filteredActivities = await ActivityService.GetFilteredAsync(activityFilterModel);

        Activities = filteredActivities.Activity ?? new List<ActivityResponseModel>();

        TotalPages = filteredActivities.TotalPages;
        if (activityFilterModel.CurrentPage > TotalPages && TotalPages > 0)
            activityFilterModel.CurrentPage = TotalPages;
        if (activityFilterModel.CurrentPage < 1)
            activityFilterModel.CurrentPage = 1;
    }

    protected async Task OnFilter()
    {
        activityFilterModel.CurrentPage = 1;
        await LoadActivities();
    }

    protected async Task OnReset()
    {
        activityFilterModel.Date = null;
        activityFilterModel.ProductTypeId = null;
        activityFilterModel.ProductId = null;
        activityFilterModel.LocationId = null;
        activityFilterModel.ActionId = null;
        activityFilterModel.Quantity = null; // Reset quantity filter
        activityFilterModel.CurrentPage = 1;
        await LoadActivities();
    }

    private async Task LoadLookups()
    {
        var lookupsList = await LookupsService.GetAllAsync();
        Lookups = lookupsList.FirstOrDefault();
    }

    protected async Task ShowAddForm()
    {
        var localNow = await JavascriptMethodsService.GetLocalDateTimeAsync();
        EditActivity = new ActivityEditModel() { ActivityDate = localNow };
        ShowForm = true;
    }

    //protected void Edit(ActivityResponseModel activity)
    //{
    //    EditActivity = new ActivityEditModel
    //    {
    //        Id = activity.Id,
    //        ActivityDate = activity.ActivityDate,
    //        ActionId = activity.ActionId,
    //        ProductId = activity.ProductId,
    //        ProductTypeId = activity.ProductTypeId,
    //        LocationId = activity.LocationId,
    //        Quantity = activity.Quantity,
    //        Notes = activity.Notes,
    //        Deleted = activity.Deleted
    //    };
    //    ShowForm = true;
    //}

    protected void OpenStockSale(int stockSaleId)
    {
        Navigation.NavigateTo($"/stock-sale/{stockSaleId}");
    }

    protected void OpenStockOrder(int stockReceiptId)
    {
        Navigation.NavigateTo($"/stock-order/{stockReceiptId}");
    }

    protected void OpenDeliveryNote(int deliveryNoteId)
    {
        Navigation.NavigateTo($"/delivery-note/{deliveryNoteId}");
    }

    //protected async Task HandleValidSubmit()
    //{
    //    if (EditActivity.Id == 0)
    //    {
    //        var newId = await ActivityService.CreateAsync(EditActivity);
    //        Activities.Insert(
    //            0,
    //            new ActivityResponseModel()
    //            {
    //                Id = newId,
    //                ActionId = EditActivity.ActionId,
    //                ActionName = Lookups.ActionList.Where(a => a.Id == EditActivity.ActionId).FirstOrDefault()!.ActionName,
    //                ActivityDate = EditActivity.ActivityDate,
    //                ProductId = EditActivity.ProductId,
    //                ProductTypeId = EditActivity.ProductTypeId,
    //                LocationId = EditActivity.LocationId,
    //                Quantity = EditActivity.Quantity,
    //                Notes = EditActivity.Notes,
    //                ProductName = Lookups.ProductList.Where(p => p.Id == EditActivity.ProductId).FirstOrDefault()!.ProductName,
    //                ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditActivity.ProductTypeId).FirstOrDefault()!.ProductTypeName,
    //                LocationName = Lookups.LocationList.Where(v => v.Id == EditActivity.LocationId).FirstOrDefault()!.Name,
    //                Deleted = false
    //            });
    //    }
    //    else
    //    {
    //        await ActivityService.UpdateAsync(EditActivity);
    //        var index = Activities.FindIndex(p => p.Id == EditActivity.Id);
    //        if (index >= 0)
    //        {
    //            Activities[index] = new ActivityResponseModel()
    //            {
    //                Id = EditActivity.Id,
    //                ActionId = EditActivity.ActionId,
    //                ActionName = Lookups.ActionList.Where(a => a.Id == EditActivity.ActionId).FirstOrDefault()!.ActionName,
    //                ActivityDate = EditActivity.ActivityDate,
    //                ProductId = EditActivity.ProductId,
    //                ProductTypeId = EditActivity.ProductTypeId,
    //                LocationId = EditActivity.LocationId,
    //                Quantity = EditActivity.Quantity,
    //                Notes = EditActivity.Notes,
    //                ProductName = Lookups.ProductList.Where(p => p.Id == EditActivity.ProductId).FirstOrDefault()!.ProductName,
    //                ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditActivity.ProductTypeId).FirstOrDefault()!.ProductTypeName,
    //                LocationName = Lookups.LocationList.Where(v => v.Id == EditActivity.LocationId).FirstOrDefault()!.Name,
    //                Deleted = false
    //            };
    //        }
    //    }
    //    ShowForm = false;
    //}

    //protected void CancelEdit()
    //{
    //    ShowForm = false;
    //}

    //protected void Delete(int id)
    //{
    //    EditActivity = new ActivityEditModel { Id = id };
    //    ShowDeleteConfirm = true;
    //}

    //protected async Task HandleDeleteConfirmation(bool confirmed)
    //{
    //    ShowDeleteConfirm = false;
    //    if (confirmed)
    //    {
    //        await ActivityService.DeleteAsync(EditActivity.Id);
    //        Activities.RemoveAll(a => a.Id == EditActivity.Id);
    //        EditActivity = new ActivityEditModel();
    //    }
    //}

    protected async Task GoToPage(int page)
    {
        if (page < 1 || page > TotalPages)
            return;
        activityFilterModel.CurrentPage = page;
        await LoadActivities();
    }

    protected void ToggleFilters()
    {
        filtersExpanded = !filtersExpanded;
    }
}

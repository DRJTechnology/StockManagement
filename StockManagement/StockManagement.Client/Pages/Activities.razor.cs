using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Services;
using StockManagement.Models;

[Authorize]
public partial class ActivitiesBase : ComponentBase
{
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
    protected bool IsLoading { get; set; }
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
        //var allActivities = (await ActivityService.GetAllAsync())?.ToList() ?? new();
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
        activityFilterModel.VenueId = null;
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

    protected void Edit(ActivityResponseModel activity)
    {
        EditActivity = new ActivityEditModel
        {
            Id = activity.Id,
            ActivityDate = activity.ActivityDate,
            ActionId = activity.ActionId,
            ProductId = activity.ProductId,
            ProductTypeId = activity.ProductTypeId,
            VenueId = activity.VenueId,
            Quantity = activity.Quantity,
            Deleted = activity.Deleted
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        if (EditActivity.Id == 0)
        {
            var newId = await ActivityService.CreateAsync(EditActivity);
            Activities.Insert(
                0,
                new ActivityResponseModel()
                {
                    Id = newId,
                    ActionId = EditActivity.ActionId,
                    ActionName = Lookups.ActionList.Where(a => a.Id == EditActivity.ActionId).FirstOrDefault()!.ActionName,
                    ActivityDate = EditActivity.ActivityDate,
                    ProductId = EditActivity.ProductId,
                    ProductTypeId = EditActivity.ProductTypeId,
                    VenueId = EditActivity.VenueId,
                    Quantity = EditActivity.Quantity,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditActivity.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditActivity.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    VenueName = Lookups.VenueList.Where(v => v.Id == EditActivity.VenueId).FirstOrDefault()!.VenueName,
                    Deleted = false
                });
        }
        else
        {
            await ActivityService.UpdateAsync(EditActivity);
            var index = Activities.FindIndex(p => p.Id == EditActivity.Id);
            if (index >= 0)
            {
                Activities[index] = new ActivityResponseModel()
                {
                    Id = EditActivity.Id,
                    ActionId = EditActivity.ActionId,
                    ActionName = Lookups.ActionList.Where(a => a.Id == EditActivity.ActionId).FirstOrDefault()!.ActionName,
                    ActivityDate = EditActivity.ActivityDate,
                    ProductId = EditActivity.ProductId,
                    ProductTypeId = EditActivity.ProductTypeId,
                    VenueId = EditActivity.VenueId,
                    Quantity = EditActivity.Quantity,
                    ProductName = Lookups.ProductList.Where(p => p.Id == EditActivity.ProductId).FirstOrDefault()!.ProductName,
                    ProductTypeName = Lookups.ProductTypeList.Where(pt => pt.Id == EditActivity.ProductTypeId).FirstOrDefault()!.ProductTypeName,
                    VenueName = Lookups.VenueList.Where(v => v.Id == EditActivity.VenueId).FirstOrDefault()!.VenueName,
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
        await ActivityService.DeleteAsync(id);
        Activities.RemoveAll(a => a.Id == id);
    }

    protected async Task GoToPage(int page)
    {
        if (page < 1 || page > TotalPages)
            return;
        activityFilterModel.CurrentPage = page;
        await LoadActivities();
    }

    protected async Task PreviousPage()
    {
        if (activityFilterModel.CurrentPage > 1)
        {
            activityFilterModel.CurrentPage--;
            await LoadActivities();
        }
    }

    protected async Task NextPage()
    {
        if (activityFilterModel.CurrentPage < TotalPages)
        {
            activityFilterModel.CurrentPage++;
            await LoadActivities();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Client.Pages;
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

    protected List<ActivityResponseModel> Activities { get; set; } = new();
    protected ActivityEditModel EditActivity { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool IsLoading { get; set; }
    public LookupsModel Lookups { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadActivities();
            await LoadLookups();
            IsLoading = false;
        }
    }

    protected async Task LoadActivities()
    {
        Activities = (await ActivityService.GetAllAsync())?.ToList() ?? new();
    }

    private async Task LoadLookups()
    {
        var lookupsList = await LookupsService.GetAllAsync();
        Lookups = lookupsList.FirstOrDefault();
    }


    protected void ShowAddForm()
    {
        EditActivity = new ActivityEditModel() { ActivityDate = DateTime.Today };
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
            Activities.Add(
                new ActivityResponseModel()
                {
                    Id = newId,
                    ActionId = EditActivity.ActionId,
                    ActionName = "",
                    ActivityDate = EditActivity.ActivityDate,
                    ProductId = EditActivity.ProductId,
                    ProductTypeId = EditActivity.ProductTypeId,
                    VenueId = EditActivity.VenueId,
                    Quantity = EditActivity.Quantity,
                    ProductName = "",
                    ProductTypeName = "",
                    VenueName = "",
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
                    ActionName = "",
                    ActivityDate = EditActivity.ActivityDate,
                    ProductId = EditActivity.ProductId,
                    ProductTypeId = EditActivity.ProductTypeId,
                    VenueId = EditActivity.VenueId,
                    Quantity = EditActivity.Quantity,
                    ProductName = "",
                    ProductTypeName = "",
                    VenueName = "",
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
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
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

    // Filter properties
    protected DateTime? FilterDate { get; set; }
    protected int? FilterProductTypeId { get; set; }
    protected int? FilterProductId { get; set; }
    protected int? FilterVenueId { get; set; }
    protected int? FilterActionId { get; set; }
    protected int? FilterQuantity { get; set; } // Added for quantity filter

    // Pagination properties
    protected int CurrentPage { get; set; } = 1;
    protected int PageSize { get; set; } = 25;
    protected int TotalPages { get; set; }
    protected List<ActivityResponseModel> PagedActivities { get; set; } = new();

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
        var allActivities = (await ActivityService.GetAllAsync())?.ToList() ?? new();

        Activities = allActivities
            .Where(a =>
                (!FilterDate.HasValue || a.ActivityDate.Date == FilterDate.Value.Date) &&
                (!FilterProductTypeId.HasValue || FilterProductTypeId == 0 || a.ProductTypeId == FilterProductTypeId) &&
                (!FilterProductId.HasValue || FilterProductId == 0 || a.ProductId == FilterProductId) &&
                (!FilterVenueId.HasValue || FilterVenueId == 0 || a.VenueId == FilterVenueId) &&
                (!FilterActionId.HasValue || FilterActionId == 0 || a.ActionId == FilterActionId) &&
                (!FilterQuantity.HasValue || a.Quantity == FilterQuantity)
            )
            .ToList();

        TotalPages = (int)Math.Ceiling(Activities.Count / (double)PageSize);
        if (CurrentPage > TotalPages && TotalPages > 0)
            CurrentPage = TotalPages;
        if (CurrentPage < 1)
            CurrentPage = 1;

        PagedActivities = Activities
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    protected async Task OnFilter()
    {
        CurrentPage = 1;
        await LoadActivities();
    }

    protected async Task OnReset()
    {
        FilterDate = null;
        FilterProductTypeId = null;
        FilterProductId = null;
        FilterVenueId = null;
        FilterActionId = null;
        FilterQuantity = null; // Reset quantity filter
        CurrentPage = 1;
        await LoadActivities();
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
        CurrentPage = page;
        await LoadActivities();
    }

    protected async Task PreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadActivities();
        }
    }

    protected async Task NextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadActivities();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;

[Authorize]
public partial class SettingsBase : ComponentBase
{
    [Inject]
    protected ISettingDataService SettingService { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    protected List<SettingResponseModel> Settings { get; set; } = new();
    protected SettingEditModel EditSetting { get; set; } = new();
    protected bool ShowForm { get; set; }
    protected bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (JSRuntime is IJSInProcessRuntime)
        {
            await LoadSettings();
        }
    }

    protected async Task LoadSettings()
    {
        Settings = (await SettingService.GetAllAsync())?.ToList() ?? new();
        IsLoading = false;
    }

    protected void ShowAddForm()
    {
        EditSetting = new SettingEditModel();
        ShowForm = true;
    }

    protected void Edit(SettingResponseModel setting)
    {
        EditSetting = new SettingEditModel
        {
            Id = setting.Id,
            SettingName = setting.SettingName,
            SettingValue = setting.SettingValue,
        };
        ShowForm = true;
    }

    protected async Task HandleValidSubmit()
    {
        await SettingService.UpdateAsync(EditSetting);
        var index = Settings.FindIndex(v => v.Id == EditSetting.Id);
        if (index >= 0)
        {
            Settings[index] = new SettingResponseModel()
            {
                Id = EditSetting.Id,
                SettingName = EditSetting.SettingName,
                SettingValue = EditSetting.SettingValue,
                Deleted = false
            };
        }

        ShowForm = false;
    }

    protected void CancelEdit()
    {
        ShowForm = false;
    }
}

namespace StockManagement.Models.Dto
{
    public class SettingDto : BaseDto
    {
        public string SettingName { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
    }
}

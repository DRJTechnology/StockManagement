namespace StockManagement.Models.Dto
{
    public class SettingDto
    {
        public int Id { get; set; }
        public string SettingName { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public bool Deleted { get; set; }
    }
}

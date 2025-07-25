namespace StockManagement.Models
{
    public class ActivityResponseModel : ActivityEditModel
    {
        public string ActionName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
    }
}

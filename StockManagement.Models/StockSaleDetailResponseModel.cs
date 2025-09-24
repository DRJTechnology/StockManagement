namespace StockManagement.Models
{
    public class StockSaleDetailResponseModel : StockSaleDetailEditModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
    }
}

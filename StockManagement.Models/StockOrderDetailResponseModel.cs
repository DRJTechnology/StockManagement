namespace StockManagement.Models
{
    public class StockOrderDetailResponseModel : StockOrderDetailEditModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
    }
}

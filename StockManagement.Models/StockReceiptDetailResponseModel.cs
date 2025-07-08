namespace StockManagement.Models
{
    public class StockReceiptDetailResponseModel : StockReceiptDetailEditModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
    }
}

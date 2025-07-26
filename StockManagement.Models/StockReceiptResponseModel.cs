namespace StockManagement.Models
{
    public class StockReceiptResponseModel : StockReceiptEditModel
    {
        public string? ContactName { get; set; }
        public List<StockReceiptDetailResponseModel> DetailList { get; set; }
    }
}

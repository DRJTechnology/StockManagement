namespace StockManagement.Models
{
    public class StockReceiptResponseModel : StockReceiptEditModel
    {
        public string? VenueName { get; set; }
        public List<StockReceiptDetailResponseModel> DetailList { get; set; }
    }
}

namespace StockManagement.Models
{
    public class StockOrderResponseModel : StockOrderEditModel
    {
        public string? ContactName { get; set; }
        public List<StockOrderDetailResponseModel> DetailList { get; set; }
        public bool PaymentRecorded { get; set; }
        public bool StockReceiptRecorded { get; set; }
    }
}

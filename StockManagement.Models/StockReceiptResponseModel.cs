namespace StockManagement.Models
{
    public class StockReceiptResponseModel : StockReceiptEditModel
    {
        public string? SupplierName { get; set; }
        public List<StockReceiptDetailResponseModel> DetailList { get; set; }
    }
}

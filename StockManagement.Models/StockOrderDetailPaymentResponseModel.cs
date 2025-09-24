namespace StockManagement.Models
{
    public class StockOrderDetailPaymentResponseModel : StockOrderDetailResponseModel
    {
        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }
    }
}

namespace StockManagement.Models
{
    public class StockOrderPaymentsCreateModel
    {
        public int StockOrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ContactId { get; set; }
        public decimal Cost { get; set; }
        public List<StockOrderDetailPaymentResponseModel> StockOrderDetailPayments { get; set; } = new();
    }
}

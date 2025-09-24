namespace StockManagement.Models
{
    public class StockSaleConfirmPaymentModel
    {
        public int StockSaleId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

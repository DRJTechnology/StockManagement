namespace StockManagement.Models
{
    public class StockSaleConfirmationModel
    {
        public int StockSaleId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<StockSaleDetailResponseModel> StockSaleDetails { get; set; } = new();
    }
}

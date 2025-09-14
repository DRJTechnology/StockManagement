namespace StockManagement.Models
{
    public class StockSaleConfirmationModel
    {
        public int StockSaleId { get; set; }
        public decimal TotalPrice { get; set; }
        public int LocationId { get; set; }
        public int ContactId { get; set; }
        public List<StockSaleDetailResponseModel> StockSaleDetails { get; set; } = new();
    }
}

namespace StockManagement.Models
{
    public class StockSaleResponseModel : StockSaleEditModel
    {
        public string? LocationName { get; set; }
        public string? ContactName { get; set; }
        public List<StockSaleDetailResponseModel> DetailList { get; set; } = new();
    }
}

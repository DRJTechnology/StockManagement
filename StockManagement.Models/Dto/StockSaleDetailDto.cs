namespace StockManagement.Models.Dto
{
    public class StockSaleDetailDto : BaseDto
    {
        public int StockSaleId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}

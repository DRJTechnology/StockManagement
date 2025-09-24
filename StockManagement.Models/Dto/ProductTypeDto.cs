namespace StockManagement.Models.Dto
{
    public class ProductTypeDto : BaseDto
    {
        public string ProductTypeName { get; set; } = string.Empty;
        public decimal DefaultCostPrice { get; set; }
        public decimal DefaultSalePrice { get; set; }
    }
}

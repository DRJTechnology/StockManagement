namespace StockManagement.Models.Dto
{
    public class StockReceiptDetailDto : BaseDto
    {
        public int StockReceiptId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}

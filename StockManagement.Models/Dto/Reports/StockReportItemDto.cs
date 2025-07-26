namespace StockManagement.Models.Dto.Reports
{
    public class StockReportItemDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
    }
}

namespace StockManagement.Models.Dto.Reports
{
    public class StockReportItemDto
    {
        public string VenueName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
    }
}

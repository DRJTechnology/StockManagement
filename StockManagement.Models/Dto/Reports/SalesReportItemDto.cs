namespace StockManagement.Models.Dto.Reports
{
    public class SalesReportItemDto
    {
        public string LocationName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int TotalSales { get; set; }
    }
}

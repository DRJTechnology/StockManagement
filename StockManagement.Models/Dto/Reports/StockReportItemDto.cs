namespace StockManagement.Models.Dto.Reports
{
    public class StockReportItemDto
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
    }
}

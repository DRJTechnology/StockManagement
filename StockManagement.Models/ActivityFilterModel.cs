namespace StockManagement.Models
{
    public class ActivityFilterModel
    {
        // Filter properties
        public DateTime? Date { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductId { get; set; }
        public int? LocationId { get; set; }
        public int? ActionId { get; set; }
        public int? Quantity { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}

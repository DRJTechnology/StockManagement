using StockManagement.Models.Enums;

namespace StockManagement.Models
{
    public class InventoryBatchFilterModel
    {
        // Filter properties
        public InventoryBatchStatusEnum Status { get; set; } = InventoryBatchStatusEnum.Active;
        public int? ProductId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? Quantity { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}

using StockManagement.Models.Enums;

namespace StockManagement.Models
{
    public class InventoryBatchEditModel
    {
        public int Id { get; set; }
        public InventoryBatchStatusEnum InventoryBatchStatus { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int LocationId { get; set; }
        public int InitialQuantity { get; set; }
        public int QuantityRemaining { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool Deleted { get; set; }
    }
}

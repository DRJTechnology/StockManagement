using StockManagement.Models.Enums;

namespace StockManagement.Models.Dto.Finance
{
    public class InventoryBatchDto : BaseDto
    {
        public short InventoryBatchStatusId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public int InitialQuantity { get; set; }
        public int QuantityRemaining { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

namespace StockManagement.Models.Dto
{
    public class InventoryBatchDto : BaseDto
    {
        public Int16 InventoryBatchStatusId { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int LocationId { get; set; }
        public int InitialQuantity { get; set; }
        public int QuantityRemaining { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

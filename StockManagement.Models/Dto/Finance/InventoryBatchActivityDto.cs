using StockManagement.Models.Enums;

namespace StockManagement.Models.Dto.Finance
{
    public class InventoryBatchActivityDto
    {
        public short Id { get; set; }
        public int Quantity { get; set; }
        public int ActivityId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActionName { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int StockOrderId { get; set; }
        public int StockSaleId { get; set; }
    }
}

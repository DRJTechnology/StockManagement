using StockManagement.Models.Finance;

namespace StockManagement.Models
{
    public class InventoryBatchFilteredResponseModel
    {
        public int TotalPages { get; set; }
        public List<InventoryBatchResponseModel> InventoryBatchList { get; set; } = new();
    }
}

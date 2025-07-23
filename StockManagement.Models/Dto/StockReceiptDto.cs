namespace StockManagement.Models.Dto
{
    public class StockReceiptDto : BaseDto
    {
        public DateTime Date { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<StockReceiptDetailDto> DetailList { get; set; } = new List<StockReceiptDetailDto>();
    }
}

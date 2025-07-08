namespace StockManagement.Models.Dto
{
    public class StockReceiptDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public bool Deleted { get; set; }
        public List<StockReceiptDetailDto> DetailList { get; set; } = new List<StockReceiptDetailDto>();
    }
}

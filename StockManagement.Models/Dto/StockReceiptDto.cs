namespace StockManagement.Models.Dto
{
    public class StockReceiptDto : BaseDto
    {
        public DateTime Date { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public bool Deleted { get; set; }
        public List<StockReceiptDetailDto> DetailList { get; set; } = new List<StockReceiptDetailDto>();
    }
}

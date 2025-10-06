namespace StockManagement.Models.Dto
{
    public class StockOrderDto : BaseDto
    {
        public DateTime Date { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public List<StockOrderDetailDto> DetailList { get; set; } = new List<StockOrderDetailDto>();
        public decimal TotalCost { get; set; }
        public bool PaymentRecorded { get; set; }
        public bool StockReceiptRecorded { get; set; }

    }
}

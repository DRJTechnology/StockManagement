namespace StockManagement.Models.Dto
{
    public class StockOrderDto : BaseDto
    {
        public DateTime Date { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public bool Deleted { get; set; }
        public List<StockOrderDetailDto> DetailList { get; set; } = new List<StockOrderDetailDto>();
        public bool PaymentRecorded { get; set; }
        public bool StockRecorded { get; set; }

    }
}

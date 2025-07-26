namespace StockManagement.Models.Dto
{
    public class DeliveryNoteDto : BaseDto
    {
        public DateTime Date { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public bool DirectSale { get; set; }
        public List<DeliveryNoteDetailDto> DetailList { get; set; } = new List<DeliveryNoteDetailDto>();
    }
}

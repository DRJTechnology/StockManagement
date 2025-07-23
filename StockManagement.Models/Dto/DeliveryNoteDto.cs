namespace StockManagement.Models.Dto
{
    public class DeliveryNoteDto : BaseDto
    {
        public DateTime Date { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public bool DirectSale { get; set; }
        public List<DeliveryNoteDetailDto> DetailList { get; set; } = new List<DeliveryNoteDetailDto>();
    }
}

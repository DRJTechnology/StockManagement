namespace StockManagement.Models.Dto
{
    public class DeliveryNoteDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public bool Deleted { get; set; }
    }
}

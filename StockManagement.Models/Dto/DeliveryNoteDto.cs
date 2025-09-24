namespace StockManagement.Models.Dto
{
    public class DeliveryNoteDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public bool DeliveryCompleted { get; set; }
        public bool Deleted { get; set; }
        public List<DeliveryNoteDetailDto> DetailList { get; set; } = new List<DeliveryNoteDetailDto>();
    }
}

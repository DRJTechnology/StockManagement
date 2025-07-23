namespace StockManagement.Models.Dto
{
    public class ActivityDto : BaseDto
    {
        public DateTime ActivityDate { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int VenueId { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public int DeliveryNoteId { get; set; }
        public int StockReceiptId { get; set; }
        public int Quantity { get; set; }
    }
}

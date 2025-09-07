namespace StockManagement.Models.Dto
{
    public class DeliveryNoteDetailDto
    {
        public int Id { get; set; }
        public int DeliveryNoteId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool Deleted { get; set; }
    }
}

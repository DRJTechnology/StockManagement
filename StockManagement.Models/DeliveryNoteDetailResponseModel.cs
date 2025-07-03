namespace StockManagement.Models
{
    public class DeliveryNoteDetailResponseModel : DeliveryNoteDetailEditModel
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
    }
}

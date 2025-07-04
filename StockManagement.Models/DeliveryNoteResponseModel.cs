namespace StockManagement.Models
{
    public class DeliveryNoteResponseModel : DeliveryNoteEditModel
    {
        public string? VenueName { get; set; }
        public List<DeliveryNoteDetailResponseModel> DetailList { get; set; }
    }
}

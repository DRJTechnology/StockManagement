namespace StockManagement.Models
{
    public class DeliveryNoteResponseModel : DeliveryNoteEditModel
    {
        public string? LocationName { get; set; }
        public List<DeliveryNoteDetailResponseModel> DetailList { get; set; }
    }
}

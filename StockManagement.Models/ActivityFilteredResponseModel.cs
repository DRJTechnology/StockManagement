namespace StockManagement.Models
{
    public class ActivityFilteredResponseModel
    {
        public int TotalPages { get; set; }
        public List<ActivityResponseModel> Activity { get; set; } = new();
    }
}

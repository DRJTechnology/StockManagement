namespace StockManagement.Models.InternalObjects
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int CreatedId { get; set; }
        public int ExistingId { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}

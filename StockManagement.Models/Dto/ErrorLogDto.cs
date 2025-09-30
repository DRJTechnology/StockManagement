namespace StockManagement.Models.Dto
{
    public class ErrorLogDto
    {
        public string LogLevel { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public int UserId { get; set; }

    }
}

namespace StockManagement.Models.Dto.Finance
{
    public class YearEndCheckDto
    {
        public string CheckType { get; set; } = string.Empty;

        /// <summary>"Action" needs fixing before closing; "Review" is for the accountant to judge.</summary>
        public string Severity { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }

        public decimal Amount { get; set; }
    }
}

namespace StockManagement.Models.Dto.Finance
{
    public class OwnersAccountDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public decimal OpeningBalance { get; set; }

        public DateTime Date { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;

        /// <summary>Money in is positive, money out is negative.</summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Separates real cash movements from the non-cash journal for stock
        /// taken for own use.
        /// </summary>
        public string Category { get; set; } = string.Empty;
    }
}

namespace StockManagement.Models.Dto.Finance
{
    public class ProfitAndLossDto
    {
        /// <summary>1 = Income, 2 = Cost of Sales, 3 = Expenses.</summary>
        public int SectionId { get; set; }

        public string Section { get; set; } = string.Empty;

        public int AccountId { get; set; }

        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Always positive. Net profit is Income less Cost of Sales less
        /// Expenses - never a plain sum of every row.
        /// </summary>
        public decimal Amount { get; set; }
    }
}

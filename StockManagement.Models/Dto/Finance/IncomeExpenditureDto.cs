namespace StockManagement.Models.Dto.Finance
{
    public class IncomeExpenditureDto
    {
        /// <summary>1 = Income, 2 = Expenditure.</summary>
        public int SectionId { get; set; }

        public string Section { get; set; } = string.Empty;

        public int AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        /// <summary>
        /// True where no money changed hands - cost of goods sold, stock
        /// written off, stock used for promotion. These drop out under the
        /// cash basis.
        /// </summary>
        public bool IsNonCash { get; set; }
    }
}

namespace StockManagement.Models.Dto.Finance
{
    public class BalanceSheetDto
    {
        /// <summary>
        /// 1 = Assets, 2 = Liabilities, 3 = Long-term Liabilities, 4 = Capital.
        /// </summary>
        public int SectionId { get; set; }

        public string Section { get; set; } = string.Empty;

        /// <summary>Null for the computed retained earnings and profit lines.</summary>
        public int? AccountId { get; set; }

        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// Signed as it should be presented: assets and capital introduced
        /// positive, drawings negative.
        /// </summary>
        public decimal Amount { get; set; }
    }
}

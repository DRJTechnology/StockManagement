namespace StockManagement.Models.Dto.Finance
{
    public class TrialBalanceDto
    {
        public int AccountTypeId { get; set; }
        public int AccountId { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;

        /// <summary>Total of all debit postings in the period.</summary>
        public decimal Debit { get; set; }

        /// <summary>Total of all credit postings in the period.</summary>
        public decimal Credit { get; set; }

        /// <summary>Net balance, where the account is in debit.</summary>
        public decimal BalanceDebit { get; set; }

        /// <summary>Net balance, where the account is in credit.</summary>
        public decimal BalanceCredit { get; set; }
    }
}

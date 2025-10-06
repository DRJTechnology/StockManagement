namespace StockManagement.Models.Dto.Finance
{
    public class TrialBalanceDto
    {
        public int AccountTypeId { get; set; }
        public int AccountId { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}

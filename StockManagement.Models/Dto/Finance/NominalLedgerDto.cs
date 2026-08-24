namespace StockManagement.Models.Dto.Finance
{
    public class NominalLedgerDto
    {
        public int AccountTypeId { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public int AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty;

        /// <summary>0 = opening balance line, 1 = a posting.</summary>
        public int LineType { get; set; }

        public DateTime? Date { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal RunningBalance { get; set; }
    }
}

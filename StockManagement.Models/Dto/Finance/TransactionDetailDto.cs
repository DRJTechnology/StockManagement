namespace StockManagement.Models.Dto.Finance
{
    public class TransactionDetailDto : BaseDto
    {
        public int TransactionId { get; set; }

        public string Type { get; set; } = string.Empty;

        public int AccountId { get; set; }

        public string Account { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public Int16 Direction { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }
    }
}

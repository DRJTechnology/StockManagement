namespace StockManagement.Models.Finance
{
    public class TransactionFilterModel
    {
        // Filter properties
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? AccountId { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? ContactId { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}

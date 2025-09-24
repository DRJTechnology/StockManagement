namespace StockManagement.Models.Finance
{
    public class TransactionFilteredResponseModel
    {
        public int TotalPages { get; set; }
        public List<TransactionDetailResponseModel> TransactionDetailList { get; set; } = new List<TransactionDetailResponseModel>();
    }
}

namespace StockManagement.Models.Finance
{
    public class TransactionDetailResponseModel : TransactionDetailEditModel
    {
        public string Account { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
    }
}

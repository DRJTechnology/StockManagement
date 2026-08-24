namespace StockManagement.Models.Dto.Finance
{
    public class StockReconciliationDto
    {
        public int SortOrder { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        /// <summary>Renders as a subtotal / emphasised line.</summary>
        public bool IsTotal { get; set; }
    }
}

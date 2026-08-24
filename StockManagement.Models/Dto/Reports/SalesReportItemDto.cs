namespace StockManagement.Models.Dto.Reports
{
    public class SalesReportItemDto
    {
        public string LocationName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int TotalSales { get; set; }

        /// <summary>
        /// Sale value from the linked sale line. Sales recorded before the
        /// stock and finance modules went live are not linked to a sale line
        /// and so carry no value.
        /// </summary>
        public decimal SalesValue { get; set; }

        /// <summary>Cost of the stock the sale was fulfilled from.</summary>
        public decimal CostValue { get; set; }
    }
}

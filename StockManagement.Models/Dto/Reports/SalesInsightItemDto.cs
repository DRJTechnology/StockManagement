namespace StockManagement.Models.Dto.Reports
{
    /// <summary>
    /// A single sale line, unaggregated. The Sales Insights page pulls the
    /// whole period back at this grain and aggregates it in the browser, so
    /// changing chart dimension or measure costs no round trip.
    /// </summary>
    public class SalesInsightItemDto
    {
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// The sale this line belongs to, used to count sales and average them.
        /// Null for sales recorded before the stock module went live.
        /// </summary>
        public int? StockSaleId { get; set; }

        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;

        /// <summary>
        /// Null where the sale is not linked through to a financial
        /// transaction; <see cref="CustomerName"/> reads "No Customer".
        /// </summary>
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

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

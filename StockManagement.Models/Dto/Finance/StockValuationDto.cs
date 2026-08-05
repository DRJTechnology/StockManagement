namespace StockManagement.Models.Dto.Finance
{
    public class StockValuationDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        /// <summary>Stock at the FIFO cost of the batches it came from.</summary>
        public decimal CostValue { get; set; }

        /// <summary>
        /// Stock at the product type's default sale price, for the accountant
        /// to consider net realisable value. Note that self-produced originals
        /// carry no purchase cost, so their cost value is nil while their
        /// market value is not.
        /// </summary>
        public decimal MarketValue { get; set; }
    }
}

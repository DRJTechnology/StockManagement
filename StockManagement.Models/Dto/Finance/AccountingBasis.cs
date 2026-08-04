namespace StockManagement.Models.Dto.Finance
{
    /// <summary>
    /// Basis on which the Profit and Loss and Balance Sheet are prepared.
    /// </summary>
    public enum AccountingBasis
    {
        /// <summary>
        /// Stock is carried as an asset and released to the Profit and Loss as
        /// cost of goods sold when it is sold. Closing stock appears on the
        /// Balance Sheet.
        /// </summary>
        Accruals = 1,

        /// <summary>
        /// Stock is expensed when it is paid for. Cost of goods sold, stock
        /// written off, promotional use and stock taken for own use all drop
        /// out, and no closing stock is carried. HMRC's default basis for sole
        /// traders from 2024/25.
        /// </summary>
        Cash = 2,
    }
}

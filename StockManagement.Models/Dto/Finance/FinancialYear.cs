namespace StockManagement.Models.Dto.Finance
{
    /// <summary>
    /// The UK personal tax year, which is the accounting period for a sole
    /// trader: 6 April to 5 April.
    /// </summary>
    public static class FinancialYear
    {
        public const int StartMonth = 4;
        public const int StartDay = 6;

        /// <summary>
        /// The tax year containing <paramref name="date"/>, as an inclusive
        /// start and end date.
        /// </summary>
        public static (DateTime From, DateTime To) Containing(DateTime date)
        {
            var startYear = date.Month > StartMonth || (date.Month == StartMonth && date.Day >= StartDay)
                ? date.Year
                : date.Year - 1;

            var from = new DateTime(startYear, StartMonth, StartDay);
            return (from, from.AddYears(1).AddDays(-1));
        }

        /// <summary>
        /// The most recently completed tax year as at <paramref name="today"/>.
        /// This is the period the accounts are normally being prepared for.
        /// </summary>
        public static (DateTime From, DateTime To) MostRecentlyCompleted(DateTime today)
        {
            var current = Containing(today);
            return (current.From.AddYears(-1), current.To.AddYears(-1));
        }

        /// <summary>e.g. "2025/26".</summary>
        public static string Describe(DateTime from)
            => $"{from.Year}/{(from.Year + 1) % 100:00}";
    }
}

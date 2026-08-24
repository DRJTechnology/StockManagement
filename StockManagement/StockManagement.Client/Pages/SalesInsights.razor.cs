using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockManagement.Client.Interfaces;
using StockManagement.Models;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.Client.Pages
{
    /// <summary>What a chart or table is measuring.</summary>
    public enum InsightMeasure
    {
        Revenue = 1,
        Units = 2,
        GrossProfit = 3
    }

    /// <summary>What a chart or table is slicing by.</summary>
    public enum InsightDimension
    {
        Location = 1,
        Customer = 2,
        ProductType = 3,
        Product = 4
    }

    /// <summary>
    /// The whole period is fetched once at sale-line grain and every KPI, chart
    /// and table below is a LINQ pass over that one list. Changing a chart's
    /// dimension or measure therefore costs no round trip - only a recalculate.
    /// A new server call happens only when a filter changes.
    /// </summary>
    [Authorize]
    public partial class SalesInsightsBase : ComponentBase
    {
        [Inject] protected IReportDataService ReportDataService { get; set; } = default!;
        [Inject] protected ILookupsDataService LookupsService { get; set; } = default!;
        [Inject] public IJSRuntime JSRuntime { get; set; } = default!;

        public LookupsModel Lookups { get; private set; } = new LookupsModel();

        protected bool IsLoading = true;

        /// <summary>
        /// Bootstrap theme colours, so the charts sit with the rest of the UI.
        /// </summary>
        private static readonly string[] Palette =
        {
            "#0d6efd", "#20c997", "#fd7e14", "#6f42c1", "#d63384",
            "#198754", "#0dcaf0", "#ffc107", "#dc3545", "#6c757d"
        };

        // ---------------------------------------------------------------
        // Filters - each one refetches
        // ---------------------------------------------------------------

        private int _locationId;
        private int _customerId;
        private int _productTypeId;
        private int _productId;

        // Rolling twelve months, which shows a full season's trading the moment
        // the page opens.
        private DateTime _fromDate = DateTime.Today.AddYears(-1).AddDays(1);
        private DateTime _toDate = DateTime.Today;

        protected int LocationId
        {
            get => _locationId;
            set { if (_locationId != value) { _locationId = value; _ = ReloadAsync(); } }
        }

        protected int CustomerId
        {
            get => _customerId;
            set { if (_customerId != value) { _customerId = value; _ = ReloadAsync(); } }
        }

        protected int ProductTypeId
        {
            get => _productTypeId;
            set { if (_productTypeId != value) { _productTypeId = value; _ = ReloadAsync(); } }
        }

        protected int ProductId
        {
            get => _productId;
            set { if (_productId != value) { _productId = value; _ = ReloadAsync(); } }
        }

        protected DateTime FromDate
        {
            get => _fromDate;
            set { if (_fromDate != value) { _fromDate = value; _ = ReloadAsync(); } }
        }

        protected DateTime ToDate
        {
            get => _toDate;
            set { if (_toDate != value) { _toDate = value; _ = ReloadAsync(); } }
        }

        // ---------------------------------------------------------------
        // View options - recalculate only, no refetch
        // ---------------------------------------------------------------

        private InsightMeasure _trendMeasure = InsightMeasure.Revenue;
        private InsightDimension _breakdownDimension = InsightDimension.ProductType;
        private InsightDimension _rankingDimension = InsightDimension.Product;
        private InsightMeasure _rankingMeasure = InsightMeasure.Revenue;
        private bool _rankingWorst;

        protected InsightMeasure TrendMeasure
        {
            get => _trendMeasure;
            set { if (_trendMeasure != value) { _trendMeasure = value; Recalculate(); } }
        }

        protected InsightDimension BreakdownDimension
        {
            get => _breakdownDimension;
            set { if (_breakdownDimension != value) { _breakdownDimension = value; Recalculate(); } }
        }

        protected InsightDimension RankingDimension
        {
            get => _rankingDimension;
            set { if (_rankingDimension != value) { _rankingDimension = value; Recalculate(); } }
        }

        protected InsightMeasure RankingMeasure
        {
            get => _rankingMeasure;
            set { if (_rankingMeasure != value) { _rankingMeasure = value; Recalculate(); } }
        }

        protected bool RankingWorst
        {
            get => _rankingWorst;
            set { if (_rankingWorst != value) { _rankingWorst = value; Recalculate(); } }
        }

        protected static readonly InsightDimension[] AllDimensions =
        {
            InsightDimension.Product, InsightDimension.ProductType,
            InsightDimension.Customer, InsightDimension.Location
        };

        protected static readonly InsightMeasure[] AllMeasures =
        {
            InsightMeasure.Revenue, InsightMeasure.GrossProfit, InsightMeasure.Units
        };

        protected void OnBreakdownDimensionChanged(ChangeEventArgs args)
            => BreakdownDimension = (InsightDimension)int.Parse(args.Value?.ToString() ?? "1");

        protected void OnRankingDimensionChanged(ChangeEventArgs args)
            => RankingDimension = (InsightDimension)int.Parse(args.Value?.ToString() ?? "1");

        protected void OnRankingMeasureChanged(ChangeEventArgs args)
            => RankingMeasure = (InsightMeasure)int.Parse(args.Value?.ToString() ?? "1");

        // ---------------------------------------------------------------
        // Results
        // ---------------------------------------------------------------

        protected List<SalesInsightItemDto> Items = new();

        protected decimal Revenue { get; private set; }
        protected decimal CostOfSales { get; private set; }
        protected decimal GrossProfit => Revenue - CostOfSales;
        protected decimal MarginPercent => Revenue == 0 ? 0 : GrossProfit / Revenue * 100;
        protected int UnitsSold { get; private set; }
        protected int SaleCount { get; private set; }
        protected decimal AverageSaleValue => SaleCount == 0 ? 0 : Revenue / SaleCount;
        protected decimal AverageUnitPrice => ValuedUnits == 0 ? 0 : Revenue / ValuedUnits;

        /// <summary>
        /// Units whose sale line carries no price. Sales recorded before the
        /// finance module went live are not linked to a sale line, so they add
        /// volume but no revenue - which would quietly drag the averages and
        /// the margin down if it went unsaid.
        /// </summary>
        protected int UnvaluedUnits { get; private set; }
        protected int ValuedUnits => UnitsSold - UnvaluedUnits;
        protected decimal UnvaluedPercent => UnitsSold == 0 ? 0 : (decimal)UnvaluedUnits / UnitsSold * 100;

        protected object? TrendConfig { get; private set; }
        protected object? BreakdownConfig { get; private set; }
        protected object? RankingConfig { get; private set; }

        protected string TrendGranularityLabel { get; private set; } = string.Empty;

        protected List<InsightRow> RankingRows { get; private set; } = new();
        protected List<ProductRow> ProductRows { get; private set; } = new();

        /// <summary>One slice of a dimension, with the measure already applied.</summary>
        public record InsightRow(string Name, decimal Value, decimal Share);

        public record ProductRow(
            string ProductTypeName,
            string ProductName,
            int Quantity,
            decimal Revenue,
            decimal Cost)
        {
            public decimal GrossProfit => Revenue - Cost;
            public decimal MarginPercent => Revenue == 0 ? 0 : GrossProfit / Revenue * 100;
            public decimal AverageUnitPrice => Quantity == 0 ? 0 : Revenue / Quantity;
        }

        // ---------------------------------------------------------------
        // Loading
        // ---------------------------------------------------------------

        protected override async Task OnInitializedAsync()
        {
            // WebAssembly-only page - it deliberately does not load during prerender.
            if (JSRuntime is IJSInProcessRuntime)
            {
                await LoadLookups();
                await ReloadAsync();
            }
        }

        private async Task LoadLookups()
        {
            var lookupsList = await LookupsService.GetAllAsync();
            Lookups = lookupsList.FirstOrDefault() ?? new LookupsModel();
        }

        protected async Task ReloadAsync()
        {
            IsLoading = true;
            StateHasChanged();
            try
            {
                Items = await ReportDataService.GetSalesInsightsAsync(
                    LocationId, CustomerId, ProductTypeId, ProductId, FromDate, ToDate);
            }
            finally
            {
                IsLoading = false;
                Recalculate();
            }
        }

        protected bool HasData => Items.Count > 0;

        // ---------------------------------------------------------------
        // Aggregation
        // ---------------------------------------------------------------

        private void Recalculate()
        {
            Revenue = Items.Sum(i => i.SalesValue);
            CostOfSales = Items.Sum(i => i.CostValue);
            UnitsSold = Items.Sum(i => i.Quantity);
            UnvaluedUnits = Items.Where(i => i.SalesValue == 0).Sum(i => i.Quantity);
            SaleCount = Items.Where(i => i.StockSaleId.HasValue)
                             .Select(i => i.StockSaleId!.Value)
                             .Distinct()
                             .Count();

            BuildTrend();
            BuildBreakdown();
            BuildRanking();
            BuildProductRows();

            StateHasChanged();
        }

        private static decimal MeasureOf(IEnumerable<SalesInsightItemDto> rows, InsightMeasure measure) => measure switch
        {
            InsightMeasure.Units => rows.Sum(r => r.Quantity),
            InsightMeasure.GrossProfit => rows.Sum(r => r.SalesValue - r.CostValue),
            _ => rows.Sum(r => r.SalesValue)
        };

        protected static string MeasureLabel(InsightMeasure measure) => measure switch
        {
            InsightMeasure.Units => "Units",
            InsightMeasure.GrossProfit => "Gross profit",
            _ => "Revenue"
        };

        private static string ValueFormat(InsightMeasure measure) =>
            measure == InsightMeasure.Units ? "number" : "currency";

        private static Func<SalesInsightItemDto, string> KeyOf(InsightDimension dimension) => dimension switch
        {
            InsightDimension.Location => i => i.LocationName,
            InsightDimension.Customer => i => i.CustomerName,
            InsightDimension.ProductType => i => i.ProductTypeName,
            _ => i => i.ProductName
        };

        protected static string DimensionLabel(InsightDimension dimension) => dimension switch
        {
            InsightDimension.Location => "Location",
            InsightDimension.Customer => "Customer",
            InsightDimension.ProductType => "Product type",
            _ => "Product"
        };

        /// <summary>Every slice of a dimension, biggest first.</summary>
        private List<InsightRow> Rank(InsightDimension dimension, InsightMeasure measure)
        {
            var total = MeasureOf(Items, measure);

            return Items
                .GroupBy(KeyOf(dimension))
                .Select(g => new { g.Key, Value = MeasureOf(g, measure) })
                .Select(g => new InsightRow(
                    g.Key,
                    g.Value,
                    total == 0 ? 0 : g.Value / total * 100))
                .OrderByDescending(r => r.Value)
                .ToList();
        }

        // ---------------------------------------------------------------
        // Trend
        // ---------------------------------------------------------------

        private enum Granularity { Day, Week, Month, Year }

        private void BuildTrend()
        {
            if (!HasData)
            {
                TrendConfig = null;
                TrendGranularityLabel = string.Empty;
                return;
            }

            // Bucket across the span of the data rather than the span of the
            // filter. The shared filter offers an "All dates" button, which
            // would otherwise produce a thousand empty monthly buckets.
            var first = Items.Min(i => i.SaleDate);
            var last = Items.Max(i => i.SaleDate);
            var span = (last - first).TotalDays;

            var granularity = span <= 31 ? Granularity.Day
                            : span <= 182 ? Granularity.Week
                            : span <= 1826 ? Granularity.Month
                            : Granularity.Year;

            TrendGranularityLabel = granularity switch
            {
                Granularity.Day => "by day",
                Granularity.Week => "by week",
                Granularity.Month => "by month",
                _ => "by year"
            };

            var buckets = new List<(string Label, DateTime Start)>();
            for (var cursor = BucketStart(first, granularity);
                 cursor <= last;
                 cursor = Advance(cursor, granularity))
            {
                buckets.Add((BucketLabel(cursor, granularity), cursor));
            }

            var byBucket = Items
                .GroupBy(i => BucketStart(i.SaleDate, granularity))
                .ToDictionary(g => g.Key, g => MeasureOf(g, TrendMeasure));

            // Periods with no sales are plotted as zero, so a gap reads as a
            // quiet month rather than being skipped over.
            var values = buckets
                .Select(b => byBucket.TryGetValue(b.Start, out var v) ? v : 0m)
                .ToArray();

            TrendConfig = new
            {
                Type = "line",
                SmValueFormat = ValueFormat(TrendMeasure),
                Data = new
                {
                    Labels = buckets.Select(b => b.Label).ToArray(),
                    Datasets = new[]
                    {
                        new
                        {
                            Label = MeasureLabel(TrendMeasure),
                            Data = values,
                            BorderColor = Palette[0],
                            BackgroundColor = "rgba(13, 110, 253, 0.15)",
                            BorderWidth = 2,
                            Fill = true,
                            Tension = 0.3,
                            PointRadius = buckets.Count > 40 ? 0 : 3
                        }
                    }
                },
                Options = new
                {
                    Responsive = true,
                    MaintainAspectRatio = false,
                    Plugins = new { Legend = new { Display = false } },
                    Scales = new { Y = new { BeginAtZero = true } }
                }
            };
        }

        private static DateTime BucketStart(DateTime date, Granularity granularity) => granularity switch
        {
            Granularity.Day => date.Date,
            // Weeks run Monday to Sunday.
            Granularity.Week => date.Date.AddDays(-(((int)date.DayOfWeek + 6) % 7)),
            Granularity.Month => new DateTime(date.Year, date.Month, 1),
            _ => new DateTime(date.Year, 1, 1)
        };

        private static DateTime Advance(DateTime start, Granularity granularity) => granularity switch
        {
            Granularity.Day => start.AddDays(1),
            Granularity.Week => start.AddDays(7),
            Granularity.Month => start.AddMonths(1),
            _ => start.AddYears(1)
        };

        private static string BucketLabel(DateTime start, Granularity granularity) => granularity switch
        {
            Granularity.Day => start.ToString("d MMM"),
            Granularity.Week => start.ToString("d MMM"),
            Granularity.Month => start.ToString("MMM yy"),
            _ => start.ToString("yyyy")
        };

        // ---------------------------------------------------------------
        // Breakdown
        // ---------------------------------------------------------------

        private const int BreakdownSlices = 8;

        private void BuildBreakdown()
        {
            if (!HasData)
            {
                BreakdownConfig = null;
                return;
            }

            var ranked = Rank(BreakdownDimension, InsightMeasure.Revenue)
                .Where(r => r.Value > 0)
                .ToList();

            if (ranked.Count == 0)
            {
                BreakdownConfig = null;
                return;
            }

            var slices = ranked.Take(BreakdownSlices).ToList();
            var remainder = ranked.Skip(BreakdownSlices).Sum(r => r.Value);
            if (remainder > 0)
            {
                slices.Add(new InsightRow($"Other ({ranked.Count - BreakdownSlices})", remainder, 0));
            }

            BreakdownConfig = new
            {
                Type = "doughnut",
                SmValueFormat = "currency",
                Data = new
                {
                    Labels = slices.Select(s => s.Name).ToArray(),
                    Datasets = new[]
                    {
                        new
                        {
                            Data = slices.Select(s => s.Value).ToArray(),
                            BackgroundColor = slices.Select((_, index) => Palette[index % Palette.Length]).ToArray(),
                            BorderWidth = 0
                        }
                    }
                },
                Options = new
                {
                    Responsive = true,
                    MaintainAspectRatio = false,
                    Plugins = new { Legend = new { Position = "right" } }
                }
            };
        }

        // ---------------------------------------------------------------
        // Ranking
        // ---------------------------------------------------------------

        private const int RankingSize = 10;

        private void BuildRanking()
        {
            if (!HasData)
            {
                RankingConfig = null;
                RankingRows = new List<InsightRow>();
                return;
            }

            var ranked = Rank(RankingDimension, RankingMeasure);

            RankingRows = RankingWorst
                ? ranked.AsEnumerable().Reverse().Take(RankingSize).ToList()
                : ranked.Take(RankingSize).ToList();

            // A horizontal bar chart draws its first entry at the top, so the
            // rows go in as they are and the chart reads in the same order as
            // the table beneath it.
            var bars = RankingRows;

            RankingConfig = new
            {
                Type = "bar",
                SmValueFormat = ValueFormat(RankingMeasure),
                Data = new
                {
                    Labels = bars.Select(r => r.Name).ToArray(),
                    Datasets = new[]
                    {
                        new
                        {
                            Label = MeasureLabel(RankingMeasure),
                            Data = bars.Select(r => r.Value).ToArray(),
                            BackgroundColor = RankingWorst ? Palette[8] : Palette[1],
                            BorderWidth = 0
                        }
                    }
                },
                Options = new
                {
                    IndexAxis = "y",
                    Responsive = true,
                    MaintainAspectRatio = false,
                    Plugins = new { Legend = new { Display = false } },
                    Scales = new { X = new { BeginAtZero = true } }
                }
            };
        }

        // ---------------------------------------------------------------
        // Detail table
        // ---------------------------------------------------------------

        private void BuildProductRows()
        {
            ProductRows = Items
                .GroupBy(i => new { i.ProductTypeName, i.ProductName })
                .Select(g => new ProductRow(
                    g.Key.ProductTypeName,
                    g.Key.ProductName,
                    g.Sum(i => i.Quantity),
                    g.Sum(i => i.SalesValue),
                    g.Sum(i => i.CostValue)))
                .OrderByDescending(r => r.Revenue)
                .ThenBy(r => r.ProductName)
                .ToList();
        }

        protected string PeriodLabel => $"{FromDate:d MMMM yyyy} to {ToDate:d MMMM yyyy}";

        /// <summary>The headline figures, in the order they are shown.</summary>
        protected IEnumerable<(string Caption, string Value, string Note)> KpiTiles
        {
            get
            {
                yield return ("Revenue", Revenue.ToString("C"), string.Empty);
                yield return ("Cost of sales", CostOfSales.ToString("C"), string.Empty);
                yield return ("Gross profit", GrossProfit.ToString("C"), $"{MarginPercent:N1}% margin");
                yield return ("Units sold", UnitsSold.ToString("N0"), string.Empty);
                yield return ("Sales", SaleCount.ToString("N0"), string.Empty);
                yield return ("Average sale", AverageSaleValue.ToString("C"), string.Empty);
                yield return ("Average unit price", AverageUnitPrice.ToString("C"), $"over {ValuedUnits:N0} priced units");
                yield return ("Product lines", ProductRows.Count.ToString("N0"), $"across {Items.Select(i => i.LocationName).Distinct().Count():N0} location(s)");
            }
        }
    }
}

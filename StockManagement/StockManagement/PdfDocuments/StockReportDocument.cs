using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Reports;

namespace StockManagement.PdfDocuments
{
    /// <summary>
    /// The stock held at a single location, printed as a tally sheet: a tick box
    /// for every individual item, so a sale can be marked off on paper while it
    /// happens and entered into the system afterwards.
    /// </summary>
    public class StockReportDocument : FinanceReportDocument
    {
        /// <summary>
        /// Above this many items a row of boxes stops being useful (and stops
        /// fitting), so the item gets a blank line to write on instead.
        /// </summary>
        private const int MaxTickBoxes = 40;

        private readonly List<StockReportItemDto> items;
        private readonly string locationName;

        public StockReportDocument(List<StockReportItemDto> items, string locationName,
                                   byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.locationName = locationName;
        }

        protected override string Title => $"Stock at {locationName}";

        protected override string PeriodDescription => $"As at {DateTime.Today:d MMMM yyyy}";

        protected override string SubHeading => "Tick a box for each item sold";

        protected override void ComposeBody(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(4);
                    columns.ConstantColumn(45);
                    columns.RelativeColumn(7);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCell).Text("Product");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Qty");
                    header.Cell().Element(HeaderCell).PaddingLeft(8).Text("Sold");
                });

                foreach (var productType in items.GroupBy(i => i.ProductTypeName))
                {
                    table.Cell().ColumnSpan(3).Element(SectionCell).Text(productType.Key);

                    foreach (var item in productType)
                    {
                        table.Cell().Element(BodyCell).Text(item.ProductName);
                        table.Cell().Element(BodyCell).AlignRight().Text(Quantity(item));
                        table.Cell().Element(BodyCell).PaddingLeft(8)
                             .Element(cell => TickBoxes(cell, item.ActiveQuantity));
                    }

                    table.Cell().Element(TotalCell).Text($"{productType.Key} total");
                    table.Cell().Element(TotalCell).AlignRight()
                         .Text(productType.Sum(i => i.ActiveQuantity).ToString());
                    table.Cell().Element(TotalCell).Text("");
                }
            });
        }

        /// <summary>Active quantity, with any pending quantity in brackets - as the screen shows it.</summary>
        private static string Quantity(StockReportItemDto item)
            => item.PendingQuantity > 0
                ? $"{item.ActiveQuantity} ({item.PendingQuantity})"
                : item.ActiveQuantity.ToString();

        private static void TickBoxes(IContainer container, int quantity)
        {
            if (quantity <= 0)
            {
                container.Text("");
                return;
            }

            if (quantity > MaxTickBoxes)
            {
                container.PaddingTop(2).PaddingRight(4).Height(12)
                         .BorderBottom(1).BorderColor(Colors.Grey.Medium);
                return;
            }

            container.Inlined(inlined =>
            {
                inlined.Spacing(3);

                for (var i = 0; i < quantity; i++)
                {
                    inlined.Item().Width(11).Height(11).Border(1).BorderColor(Colors.Grey.Medium);
                }
            });
        }
    }
}

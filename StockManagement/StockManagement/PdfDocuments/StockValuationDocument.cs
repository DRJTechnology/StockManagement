using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class StockValuationDocument : FinanceReportDocument
    {
        private readonly List<StockValuationDto> items;
        private readonly DateTime asAtDate;

        public StockValuationDocument(List<StockValuationDto> items, DateTime asAtDate,
                                      byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.asAtDate = asAtDate;
        }

        protected override string Title => "Stock Valuation";

        protected override string PeriodDescription => $"As at {asAtDate:d MMMM yyyy}";

        protected override string SubHeading => "Valued at cost (FIFO)";

        protected override void ComposeBody(IContainer container)
        {
            var nilCostQuantity = items.Where(i => i.CostValue == 0).Sum(i => i.Quantity);
            var nilCostMarket = items.Where(i => i.CostValue == 0).Sum(i => i.MarketValue);

            container.Column(column =>
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(5);
                        columns.ConstantColumn(50);
                        columns.ConstantColumn(75);
                        columns.ConstantColumn(80);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCell).Text("Product Type");
                        header.Cell().Element(HeaderCell).Text("Product");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Qty");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Cost");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Market value");
                    });

                    foreach (var location in items.GroupBy(i => i.LocationName))
                    {
                        table.Cell().ColumnSpan(5).Element(SectionCell).Text(location.Key);

                        foreach (var item in location)
                        {
                            table.Cell().Element(BodyCell).Text(item.ProductTypeName);
                            table.Cell().Element(BodyCell).Text(item.ProductName);
                            table.Cell().Element(BodyCell).AlignRight().Text(item.Quantity.ToString());
                            table.Cell().Element(BodyCell).AlignRight().Text(Money(item.CostValue));
                            table.Cell().Element(BodyCell).AlignRight().Text(Money(item.MarketValue));
                        }

                        table.Cell().ColumnSpan(2).Element(TotalCell).Text($"{location.Key} total");
                        table.Cell().Element(TotalCell).AlignRight().Text(location.Sum(i => i.Quantity).ToString());
                        table.Cell().Element(TotalCell).AlignRight().Text(Money(location.Sum(i => i.CostValue)));
                        table.Cell().Element(TotalCell).AlignRight().Text(Money(location.Sum(i => i.MarketValue)));
                    }

                    table.Cell().ColumnSpan(2).Element(TotalCell).PaddingTop(8)
                         .Text("Closing stock at cost").FontSize(12).Bold();
                    table.Cell().Element(TotalCell).PaddingTop(8).AlignRight()
                         .Text(items.Sum(i => i.Quantity).ToString()).FontSize(12).Bold();
                    table.Cell().Element(TotalCell).PaddingTop(8).AlignRight()
                         .Text(Money(items.Sum(i => i.CostValue))).FontSize(12).Bold();
                    table.Cell().Element(TotalCell).PaddingTop(8).AlignRight()
                         .Text(Money(items.Sum(i => i.MarketValue))).FontSize(12).Bold();
                });

                if (nilCostQuantity > 0)
                {
                    column.Item().PaddingTop(15).Text(
                        $"Note: {nilCostQuantity} item(s) are held at nil cost (market value " +
                        $"{Money(nilCostMarket)}). These have no purchase cost recorded - typically " +
                        "original works where the materials were expensed as they were bought. Stock " +
                        "is normally carried at the lower of cost and net realisable value.")
                        .FontSize(8).Italic();
                }
            });
        }
    }
}

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class IncomeExpenditureDocument : FinanceReportDocument
    {
        private readonly List<IncomeExpenditureDto> items;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public IncomeExpenditureDocument(List<IncomeExpenditureDto> items, DateTime fromDate, DateTime toDate,
                                         byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        protected override string Title => "Income and Expenditure";

        protected override string PeriodDescription =>
            $"For the period {fromDate:d MMMM yyyy} to {toDate:d MMMM yyyy}";

        protected override void ComposeBody(IContainer container)
        {
            var nonCashTotal = items.Where(i => i.IsNonCash).Sum(i => i.Amount);

            container.Column(column =>
            {
                column.Spacing(10);

                foreach (var sectionId in new[] { 1, 2 })
                {
                    var sectionItems = items.Where(i => i.SectionId == sectionId).ToList();
                    if (sectionItems.Count == 0)
                    {
                        continue;
                    }

                    var sectionName = sectionId == 1 ? "Income" : "Expenditure";

                    column.Item().PaddingTop(6).Text(sectionName).FontSize(12).Bold();

                    foreach (var account in sectionItems.GroupBy(i => i.AccountName))
                    {
                        column.Item().Column(accountColumn =>
                        {
                            accountColumn.Item().PaddingTop(4).Text(account.Key).FontSize(10).SemiBold();

                            accountColumn.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(55);
                                    columns.RelativeColumn(7);
                                    columns.RelativeColumn(3);
                                    columns.ConstantColumn(75);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(HeaderCell).Text("Date");
                                    header.Cell().Element(HeaderCell).Text("Description");
                                    header.Cell().Element(HeaderCell).Text("Contact");
                                    header.Cell().Element(HeaderCell).AlignRight().Text("Amount");
                                });

                                foreach (var line in account)
                                {
                                    table.Cell().Element(BodyCell).Text(line.Date.ToString("dd/MM/yy"));
                                    table.Cell().Element(BodyCell)
                                         .Text(line.IsNonCash ? $"{line.Description}  [non-cash]" : line.Description);
                                    table.Cell().Element(BodyCell).Text(line.ContactName);
                                    table.Cell().Element(BodyCell).AlignRight().Text(Money(line.Amount));
                                }

                                table.Cell().ColumnSpan(3).Element(TotalCell).Text($"{account.Key} total");
                                table.Cell().Element(TotalCell).AlignRight().Text(Money(account.Sum(l => l.Amount)));
                            });
                        });
                    }

                    column.Item().PaddingTop(4).Row(row =>
                    {
                        row.RelativeItem().Text($"Total {sectionName.ToLowerInvariant()}").FontSize(11).Bold();
                        row.ConstantItem(90).AlignRight()
                           .Text(Money(sectionItems.Sum(i => i.Amount))).FontSize(11).Bold();
                    });
                }

                if (nonCashTotal != 0)
                {
                    column.Item().PaddingTop(12).Text(
                        $"Note: {Money(nonCashTotal)} of the above is non-cash - stock consumed rather " +
                        "than money spent (cost of goods sold, stock written off, stock used for " +
                        "promotion). These lines are excluded under the cash basis.")
                        .FontSize(8).Italic();
                }
            });
        }
    }
}

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class OwnersAccountDocument : FinanceReportDocument
    {
        private readonly List<OwnersAccountDto> items;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public OwnersAccountDocument(List<OwnersAccountDto> items, DateTime fromDate, DateTime toDate,
                                     byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        protected override string Title => "Owner's Capital and Drawings";

        protected override string PeriodDescription =>
            $"For the period {fromDate:d MMMM yyyy} to {toDate:d MMMM yyyy}";

        protected override void ComposeBody(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(10);

                column.Item().Text("Summary").FontSize(11).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(6);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(90);
                    });

                    foreach (var category in items.GroupBy(i => i.Category).OrderBy(g => g.Key))
                    {
                        table.Cell().Element(BodyCell).Text(category.Key);
                        table.Cell().Element(BodyCell).AlignRight().Text($"{category.Count()} item(s)");
                        table.Cell().Element(BodyCell).AlignRight().Text(Money(category.Sum(i => i.Amount)));
                    }

                    table.Cell().ColumnSpan(2).Element(TotalCell).Text("Net movement");
                    table.Cell().Element(TotalCell).AlignRight().Text(Money(items.Sum(i => i.Amount)));
                });

                foreach (var account in items.GroupBy(i => i.AccountName))
                {
                    column.Item().Column(accountColumn =>
                    {
                        accountColumn.Item().PaddingTop(6).Text(account.Key).FontSize(11).SemiBold();

                        accountColumn.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(55);
                                columns.RelativeColumn(6);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(4);
                                columns.ConstantColumn(75);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("Date");
                                header.Cell().Element(HeaderCell).Text("Description");
                                header.Cell().Element(HeaderCell).Text("Contact");
                                header.Cell().Element(HeaderCell).Text("Category");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Amount");
                            });

                            table.Cell().ColumnSpan(4).Element(BodyCell).Text("Opening balance").Italic();
                            table.Cell().Element(BodyCell).AlignRight().Text(Money(account.First().OpeningBalance));

                            foreach (var line in account)
                            {
                                table.Cell().Element(BodyCell).Text(line.Date.ToString("dd/MM/yy"));
                                table.Cell().Element(BodyCell).Text(line.Description);
                                table.Cell().Element(BodyCell).Text(line.ContactName);
                                table.Cell().Element(BodyCell).Text(line.Category);
                                table.Cell().Element(BodyCell).AlignRight().Text(Money(line.Amount));
                            }

                            table.Cell().ColumnSpan(4).Element(TotalCell).Text("Movement in period");
                            table.Cell().Element(TotalCell).AlignRight().Text(Money(account.Sum(l => l.Amount)));
                        });
                    });
                }

                column.Item().PaddingTop(12).Text(
                    "The business has no separate bank account. Money the owner puts in is capital " +
                    "introduced, and money taken out - including sale proceeds received personally - " +
                    "is drawings. This is the only record of cash flow between the owner and the business.")
                    .FontSize(8).Italic();
            });
        }
    }
}

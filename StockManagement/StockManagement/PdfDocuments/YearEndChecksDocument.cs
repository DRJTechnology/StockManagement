using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class YearEndChecksDocument : FinanceReportDocument
    {
        private readonly List<YearEndCheckDto> items;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public YearEndChecksDocument(List<YearEndCheckDto> items, DateTime fromDate, DateTime toDate,
                                     byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        protected override string Title => "Year End Checks";

        protected override string PeriodDescription =>
            $"For the period {fromDate:d MMMM yyyy} to {toDate:d MMMM yyyy}";

        protected override void ComposeBody(IContainer container)
        {
            container.Column(column =>
            {
                if (items.Count == 0)
                {
                    column.Item().Text(
                        "No cut-off issues found. Nothing straddles the year end, every confirmed " +
                        "sale has been paid, and all stock paid for has been received.");
                    return;
                }

                column.Spacing(10);

                foreach (var check in items.GroupBy(i => i.CheckType))
                {
                    column.Item().Column(checkColumn =>
                    {
                        checkColumn.Item().Text(check.Key).FontSize(11).SemiBold();

                        checkColumn.Item().PaddingTop(3).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn(8);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(70);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("Severity");
                                header.Cell().Element(HeaderCell).Text("Details");
                                header.Cell().Element(HeaderCell).Text("Date");
                                header.Cell().Element(HeaderCell).Text("Other date");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Amount");
                            });

                            foreach (var item in check)
                            {
                                table.Cell().Element(BodyCell).Text(item.Severity);
                                table.Cell().Element(BodyCell).Text(item.Details);
                                table.Cell().Element(BodyCell)
                                     .Text(item.Date1.HasValue ? item.Date1.Value.ToString("dd/MM/yy") : "");
                                table.Cell().Element(BodyCell)
                                     .Text(item.Date2.HasValue ? item.Date2.Value.ToString("dd/MM/yy") : "");
                                table.Cell().Element(BodyCell).AlignRight().Text(Money(item.Amount));
                            }
                        });
                    });
                }
            });
        }
    }
}

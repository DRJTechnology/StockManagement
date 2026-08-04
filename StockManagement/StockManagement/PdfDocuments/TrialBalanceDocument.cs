using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class TrialBalanceDocument : FinanceReportDocument
    {
        private readonly List<TrialBalanceDto> items;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public TrialBalanceDocument(List<TrialBalanceDto> items, DateTime fromDate, DateTime toDate,
                                    byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        protected override string Title => "Trial Balance";

        protected override string PeriodDescription =>
            $"For the period {fromDate:d MMMM yyyy} to {toDate:d MMMM yyyy}";

        protected override void ComposeBody(IContainer container)
        {
            var totalDebit = items.Sum(i => i.Debit);
            var totalCredit = items.Sum(i => i.Credit);

            container.Column(column =>
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(5);
                        columns.RelativeColumn(3);
                        columns.ConstantColumn(75);
                        columns.ConstantColumn(75);
                        columns.ConstantColumn(75);
                        columns.ConstantColumn(75);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCell).Text("Account");
                        header.Cell().Element(HeaderCell).Text("Type");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Debit");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Credit");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Balance Dr");
                        header.Cell().Element(HeaderCell).AlignRight().Text("Balance Cr");
                    });

                    foreach (var item in items)
                    {
                        table.Cell().Element(BodyCell).Text(item.AccountName);
                        table.Cell().Element(BodyCell).Text(item.AccountType);
                        table.Cell().Element(BodyCell).AlignRight().Text(item.Debit > 0 ? Money(item.Debit) : "");
                        table.Cell().Element(BodyCell).AlignRight().Text(item.Credit > 0 ? Money(item.Credit) : "");
                        table.Cell().Element(BodyCell).AlignRight().Text(item.BalanceDebit > 0 ? Money(item.BalanceDebit) : "");
                        table.Cell().Element(BodyCell).AlignRight().Text(item.BalanceCredit > 0 ? Money(item.BalanceCredit) : "");
                    }

                    table.Cell().ColumnSpan(2).Element(TotalCell).Text("Total");
                    table.Cell().Element(TotalCell).AlignRight().Text(Money(totalDebit));
                    table.Cell().Element(TotalCell).AlignRight().Text(Money(totalCredit));
                    table.Cell().Element(TotalCell).AlignRight().Text(Money(items.Sum(i => i.BalanceDebit)));
                    table.Cell().Element(TotalCell).AlignRight().Text(Money(items.Sum(i => i.BalanceCredit)));
                });

                if (Math.Round(totalDebit, 2) != Math.Round(totalCredit, 2))
                {
                    column.Item().PaddingTop(15).Text(
                        $"WARNING: the trial balance does not balance. Debits of {Money(totalDebit)} " +
                        $"do not agree to credits of {Money(totalCredit)}.")
                        .FontSize(9).Bold();
                }
            });
        }
    }
}

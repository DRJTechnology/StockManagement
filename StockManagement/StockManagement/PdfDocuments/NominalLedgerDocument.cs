using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class NominalLedgerDocument : FinanceReportDocument
    {
        private readonly List<NominalLedgerDto> items;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public NominalLedgerDocument(List<NominalLedgerDto> items, DateTime fromDate, DateTime toDate,
                                     byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        protected override string Title => "Nominal Ledger";

        protected override string PeriodDescription =>
            $"For the period {fromDate:d MMMM yyyy} to {toDate:d MMMM yyyy}";

        protected override void ComposeBody(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(12);

                foreach (var account in items.GroupBy(i => i.AccountName))
                {
                    column.Item().Column(accountColumn =>
                    {
                        accountColumn.Item().Text($"{account.Key}  ({account.First().AccountType})")
                                            .FontSize(11).SemiBold();

                        accountColumn.Item().PaddingTop(3).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(55);   // Date
                                columns.RelativeColumn(6);    // Description
                                columns.RelativeColumn(3);    // Contact
                                columns.ConstantColumn(65);   // Debit
                                columns.ConstantColumn(65);   // Credit
                                columns.ConstantColumn(70);   // Balance
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("Date");
                                header.Cell().Element(HeaderCell).Text("Description");
                                header.Cell().Element(HeaderCell).Text("Contact");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Debit");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Credit");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Balance");
                            });

                            foreach (var line in account)
                            {
                                table.Cell().Element(BodyCell)
                                     .Text(line.Date.HasValue ? line.Date.Value.ToString("dd/MM/yy") : "");
                                table.Cell().Element(BodyCell).Text(line.Description);
                                table.Cell().Element(BodyCell).Text(line.ContactName);
                                table.Cell().Element(BodyCell).AlignRight().Text(line.Debit > 0 ? Money(line.Debit) : "");
                                table.Cell().Element(BodyCell).AlignRight().Text(line.Credit > 0 ? Money(line.Credit) : "");
                                table.Cell().Element(BodyCell).AlignRight().Text(Money(line.RunningBalance));
                            }
                        });
                    });
                }
            });
        }
    }
}

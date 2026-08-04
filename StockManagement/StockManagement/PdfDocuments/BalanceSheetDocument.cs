using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class BalanceSheetDocument : FinanceReportDocument
    {
        private readonly List<BalanceSheetDto> items;
        private readonly DateTime toDate;
        private readonly AccountingBasis basis;

        public BalanceSheetDocument(List<BalanceSheetDto> items, DateTime toDate, AccountingBasis basis,
                                    byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.toDate = toDate;
            this.basis = basis;
        }

        protected override string Title => "Balance Sheet";

        protected override string PeriodDescription => $"As at {toDate:d MMMM yyyy}";

        protected override string SubHeading =>
            basis == AccountingBasis.Cash ? "Prepared on the cash basis" : "Prepared on the accruals basis";

        private decimal SectionTotal(int sectionId) =>
            items.Where(i => i.SectionId == sectionId).Sum(i => i.Amount);

        private decimal NetAssets => SectionTotal(1) - SectionTotal(2) - SectionTotal(3);
        private decimal TotalCapital => SectionTotal(4);
        private bool Balances => Math.Round(NetAssets, 2) == Math.Round(TotalCapital, 2);

        protected override void ComposeBody(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(6);
                        columns.ConstantColumn(90);
                    });

                    foreach (var sectionId in new[] { 1, 2, 3 })
                    {
                        var sectionItems = items.Where(i => i.SectionId == sectionId).ToList();
                        if (sectionItems.Count == 0)
                        {
                            continue;
                        }

                        table.Cell().Element(SectionCell).Text(sectionItems[0].Section);
                        table.Cell().Element(SectionCell).Text(string.Empty);

                        foreach (var item in sectionItems)
                        {
                            table.Cell().Element(BodyCell).PaddingLeft(12).Text(item.AccountName);
                            table.Cell().Element(BodyCell).AlignRight().Text(Money(item.Amount));
                        }
                    }

                    table.Cell().Element(TotalCell).Text("Net assets").Bold();
                    table.Cell().Element(TotalCell).AlignRight().Text(Money(NetAssets)).Bold();

                    table.Cell().Element(SectionCell).PaddingTop(8).Text("Capital");
                    table.Cell().Element(SectionCell).PaddingTop(8).Text(string.Empty);

                    foreach (var item in items.Where(i => i.SectionId == 4))
                    {
                        table.Cell().Element(BodyCell).PaddingLeft(12).Text(item.AccountName);
                        table.Cell().Element(BodyCell).AlignRight().Text(Money(item.Amount));
                    }

                    table.Cell().Element(TotalCell).Text("Total capital").FontSize(12).Bold();
                    table.Cell().Element(TotalCell).AlignRight().Text(Money(TotalCapital)).FontSize(12).Bold();
                });

                if (!Balances)
                {
                    column.Item().PaddingTop(15).Text(
                        $"WARNING: this balance sheet does not balance. Net assets of {Money(NetAssets)} " +
                        $"do not agree to total capital of {Money(TotalCapital)}, a difference of " +
                        $"{Money(NetAssets - TotalCapital)}.")
                        .FontSize(9).Bold();
                }

                if (basis == AccountingBasis.Cash)
                {
                    column.Item().PaddingTop(15).Text(
                        "Prepared on the cash basis: stock is expensed when paid for, so no closing " +
                        "stock is carried. With no business bank account, the business holds no " +
                        "assets and net assets are nil.")
                        .FontSize(8).Italic();
                }
            });
        }
    }
}

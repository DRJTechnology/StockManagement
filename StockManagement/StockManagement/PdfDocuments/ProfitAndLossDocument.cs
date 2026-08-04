using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class ProfitAndLossDocument : FinanceReportDocument
    {
        private readonly List<ProfitAndLossDto> items;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;
        private readonly AccountingBasis basis;

        public ProfitAndLossDocument(List<ProfitAndLossDto> items, DateTime fromDate, DateTime toDate,
                                     AccountingBasis basis, byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.basis = basis;
        }

        protected override string Title => "Profit and Loss Account";

        protected override string PeriodDescription =>
            $"For the period {fromDate:d MMMM yyyy} to {toDate:d MMMM yyyy}";

        protected override string SubHeading =>
            basis == AccountingBasis.Cash ? "Prepared on the cash basis" : "Prepared on the accruals basis";

        private decimal Income => items.Where(i => i.SectionId == 1).Sum(i => i.Amount);
        private decimal CostOfSales => items.Where(i => i.SectionId == 2).Sum(i => i.Amount);
        private decimal Expenses => items.Where(i => i.SectionId == 3).Sum(i => i.Amount);
        private decimal NetProfit => Income - CostOfSales - Expenses;

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

                    Section(table, 1, "Income");
                    Total(table, "Turnover", Income);

                    if (items.Any(i => i.SectionId == 2))
                    {
                        Section(table, 2, "Cost of Sales", bracketed: true);
                        Total(table, "Gross profit", Income - CostOfSales);
                    }

                    if (items.Any(i => i.SectionId == 3))
                    {
                        Section(table, 3, "Expenses", bracketed: true);
                        Total(table, "Total expenses", -Expenses);
                    }

                    table.Cell().Element(TotalCell).PaddingTop(8)
                         .Text(NetProfit >= 0 ? "Net profit" : "Net loss").FontSize(12).Bold();
                    table.Cell().Element(TotalCell).PaddingTop(8).AlignRight()
                         .Text(Money(NetProfit)).FontSize(12).Bold();
                });

                if (basis == AccountingBasis.Cash)
                {
                    column.Item().PaddingTop(15).Text(
                        "Prepared on the cash basis: stock is expensed when paid for. Cost of goods " +
                        "sold, stock written off, stock used for promotion and stock taken for own " +
                        "use are excluded, and no closing stock is carried on the Balance Sheet.")
                        .FontSize(8).Italic();
                }
            });
        }

        private void Section(TableDescriptor table, int sectionId, string heading, bool bracketed = false)
        {
            table.Cell().Element(SectionCell).Text(heading);
            table.Cell().Element(SectionCell).Text(string.Empty);

            foreach (var item in items.Where(i => i.SectionId == sectionId))
            {
                table.Cell().Element(BodyCell).PaddingLeft(12).Text(item.AccountName);
                table.Cell().Element(BodyCell).AlignRight()
                     .Text(bracketed ? $"({Money(item.Amount)})" : Money(item.Amount));
            }
        }

        private void Total(TableDescriptor table, string label, decimal amount)
        {
            table.Cell().Element(TotalCell).Text(label);
            table.Cell().Element(TotalCell).AlignRight()
                 .Text(amount < 0 ? $"({Money(Math.Abs(amount))})" : Money(amount));
        }
    }
}

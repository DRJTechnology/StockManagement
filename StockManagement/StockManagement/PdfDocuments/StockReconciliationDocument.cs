using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;

namespace StockManagement.PdfDocuments
{
    public class StockReconciliationDocument : FinanceReportDocument
    {
        private readonly List<StockReconciliationDto> items;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public StockReconciliationDocument(List<StockReconciliationDto> items, DateTime fromDate, DateTime toDate,
                                           byte[] logoImage, List<SettingResponseModel> settings)
            : base(logoImage, settings)
        {
            this.items = items;
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        protected override string Title => "Stock Reconciliation";

        protected override string PeriodDescription =>
            $"For the period {fromDate:d MMMM yyyy} to {toDate:d MMMM yyyy}";

        protected override void ComposeBody(IContainer container)
        {
            var difference = items.FirstOrDefault(i => i.SortOrder == 10)?.Amount ?? 0;

            container.Column(column =>
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(6);
                        columns.ConstantColumn(90);
                    });

                    foreach (var item in items)
                    {
                        var cell = item.IsTotal
                            ? new Func<IContainer, IContainer>(TotalCell)
                            : BodyCell;

                        table.Cell().Element(cell).Text(item.Description);
                        table.Cell().Element(cell).AlignRight().Text(Money(item.Amount));
                    }
                });

                column.Item().PaddingTop(15).Text(
                    Math.Abs(difference) > 0.50m
                        ? $"The Inventory account and the physical stock records differ by " +
                          $"{Money(difference)}. A few pence is FIFO rounding; anything larger " +
                          "needs investigating."
                        : $"The Inventory account agrees to the physical stock records to within " +
                          $"{Money(Math.Abs(difference))} - FIFO rounding only.")
                    .FontSize(8).Italic();
            });
        }
    }
}

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Enums;

namespace StockManagement.PdfDocuments
{
    /// <summary>
    /// Shared layout for the year-end finance reports: business name and logo,
    /// report title, the period the report covers, and a page footer. Derived
    /// documents only supply the body.
    /// </summary>
    public abstract class FinanceReportDocument : IDocument
    {
        private readonly byte[] logoImage;
        private readonly List<SettingResponseModel> settings;

        protected FinanceReportDocument(byte[] logoImage, List<SettingResponseModel> settings)
        {
            this.logoImage = logoImage;
            this.settings = settings;
        }

        protected abstract string Title { get; }

        /// <summary>e.g. "For the year 6 April 2025 to 5 April 2026".</summary>
        protected abstract string PeriodDescription { get; }

        /// <summary>Optional line under the period, e.g. the accounting basis.</summary>
        protected virtual string? SubHeading => null;

        protected abstract void ComposeBody(IContainer container);

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(ComposeHeader);
                page.Content().PaddingVertical(15).Element(ComposeBody);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Column(inner =>
                    {
                        inner.Item().Text(GetSetting(SettingEnum.BusinessName))
                                    .FontSize(16).SemiBold().FontColor(Colors.Black);
                        inner.Item().PaddingTop(2).Text(Title).FontSize(13).SemiBold();
                        inner.Item().Text(PeriodDescription).FontSize(10).FontColor(Colors.Grey.Darken2);

                        if (!string.IsNullOrEmpty(SubHeading))
                        {
                            inner.Item().Text(SubHeading).FontSize(10).FontColor(Colors.Grey.Darken2);
                        }
                    });
                    row.ConstantItem(80).AlignRight().AlignTop().Height(50).Image(logoImage);
                });
                column.Item().PaddingTop(8).BorderBottom(1).BorderColor(Colors.Grey.Medium);
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.PaddingTop(8).BorderTop(1).BorderColor(Colors.Grey.Lighten1).PaddingTop(4).Row(row =>
            {
                row.RelativeItem().AlignLeft()
                   .Text($"Prepared {DateTime.Today:d MMMM yyyy}")
                   .FontSize(8).FontColor(Colors.Grey.Darken1);

                row.RelativeItem().AlignCenter()
                   .Text(GetSetting(SettingEnum.BusinessWebsite))
                   .FontSize(8).FontColor(Colors.Blue.Medium);

                row.RelativeItem().AlignRight().Text(text =>
                {
                    text.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Grey.Darken1));
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
            });
        }

        // ---- helpers shared by the derived documents ---------------------------

        protected static IContainer HeaderCell(IContainer container) =>
            container.DefaultTextStyle(x => x.SemiBold())
                     .BorderBottom(1).BorderColor(Colors.Grey.Medium)
                     .PaddingVertical(3).PaddingHorizontal(2);

        protected static IContainer BodyCell(IContainer container) =>
            container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                     .PaddingVertical(2).PaddingHorizontal(2);

        protected static IContainer TotalCell(IContainer container) =>
            container.DefaultTextStyle(x => x.SemiBold())
                     .BorderTop(1).BorderColor(Colors.Grey.Medium)
                     .PaddingVertical(3).PaddingHorizontal(2);

        protected static IContainer SectionCell(IContainer container) =>
            container.DefaultTextStyle(x => x.SemiBold())
                     .Background(Colors.Grey.Lighten3)
                     .PaddingVertical(3).PaddingHorizontal(2);

        /// <summary>Currency in UK format, the culture the app is locked to.</summary>
        protected static string Money(decimal value) => value.ToString("C");

        protected string GetSetting(SettingEnum settingEnum)
        {
            var setting = settings.FirstOrDefault(s => s.Id == (int)settingEnum);
            return setting?.SettingValue ?? string.Empty;
        }
    }
}

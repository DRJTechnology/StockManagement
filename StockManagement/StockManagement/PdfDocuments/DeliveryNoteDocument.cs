using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Enums;

public class DeliveryNoteDocument : IDocument
{
    private readonly DeliveryNoteResponseModel deliveryNote;
    private readonly byte[] logoImage;
    private List<SettingResponseModel> settings = new();

    public DeliveryNoteDocument(DeliveryNoteResponseModel deliveryNote, byte[] logoImage, List<SettingResponseModel> settings)
    {
        this.deliveryNote = deliveryNote;
        this.logoImage = logoImage;
        this.settings = settings;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);
            page.Size(PageSizes.A5);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    private void ComposeHeader(IContainer container)
    {
        var title = "Delivery Note";

        container.Row(row =>
        {
            row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Item().PaddingTop(5).Text(title)
                                                .FontSize(20).SemiBold().FontColor(Colors.Black);
                    column.Item().Text($"Date: {deliveryNote.Date:dd/MM/yyyy}");
                });
            row.RelativeItem().AlignRight().Height(60).Image(logoImage);
        });

    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(15).Column(column =>
        {
            column.Item().PaddingTop(25).Element(ComposeToFromSection);
            column.Item().PaddingTop(25).Element(ComposeTable);
        });
    }
    private void ComposeToFromSection(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(2).Text("To:").SemiBold();
                    column.Item().Text(deliveryNote.LocationName);
                });
                row.ConstantItem(50);
                row.RelativeItem().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(2).Text("From:").SemiBold();
                    column.Item().Text(GetSetting(SettingEnum.BusinessName));
                });
            });
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3); // Product Type  
                columns.RelativeColumn(8); // Product Name  
                columns.RelativeColumn(1); // Quantity  
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Type").SemiBold();
                header.Cell().Element(CellStyle).Text("Product Name").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Qty").SemiBold();

                IContainer CellStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.FontSize(12).SemiBold());
            });

            var totalQuantity = 0;
            foreach (var item in deliveryNote.DetailList)
            {
                totalQuantity += item.Quantity;
                table.Cell().Element(CellStyle).Text(item.ProductTypeName);
                table.Cell().Element(CellStyle).Text(item.ProductName);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
            }

            IContainer CellStyle(IContainer container) =>
                container.DefaultTextStyle(x => x.FontSize(11))
                            .Border(1).Padding(2);

            // Add the totals column
            table.Cell().ColumnSpan(2).Element(TotalCellStyle).Text("Total");
            table.Cell().Element(TotalCellStyle).AlignRight().Text(totalQuantity.ToString());

            IContainer TotalCellStyle(IContainer container) =>
                container.DefaultTextStyle(x => x.FontSize(11).Bold())
                            .Border(1).Padding(2);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().AlignCenter().Text(GetSetting(SettingEnum.BusinessWebsite))
                .FontSize(10)
                .FontColor(Colors.Blue.Medium);
        });
    }

    private string GetSetting(SettingEnum settingEnum)
    {
        var setting = settings.FirstOrDefault(s => s.Id == (int)settingEnum);
        return setting?.SettingValue ?? string.Empty;
    }
}

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using StockManagement.Client.Pages;
using System.Drawing;
using StockManagement.Models;

public class DeliveryNoteDocument : IDocument
{
    private readonly DeliveryNoteResponseModel _deliveryNote;
    private readonly byte[] _logoImage;

    public DeliveryNoteDocument(DeliveryNoteResponseModel deliveryNote, byte[] logoImage)
    {
        _deliveryNote = deliveryNote;
        _logoImage = logoImage;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                //col.Item().Image(_logoImage).Height(60);

                col.Item().PaddingTop(5).Text("Delivery Note")
                    .FontSize(20).SemiBold().FontColor(Colors.Black);
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(10).Column(column =>
        {
            column.Item().Text($"Date: {_deliveryNote.Date:dd/MM/yyyy}");
            column.Item().Text($"Venue: {_deliveryNote.VenueName}");
            column.Item().PaddingTop(15).Element(ComposeTable);
        });
    }

    void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3); // Product Type  
                columns.RelativeColumn(4); // Product Name  
                columns.RelativeColumn(2); // Quantity  
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Product Type").SemiBold();
                header.Cell().Element(CellStyle).Text("Product Name").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Quantity").SemiBold();

                IContainer CellStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.FontSize(12).SemiBold())
                        .PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Medium);
            });

            foreach (var item in _deliveryNote.DetailList)
            {
                table.Cell().Element(CellStyle).Text(item.ProductTypeName);
                table.Cell().Element(CellStyle).Text(item.ProductName);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());

                IContainer CellStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.FontSize(11)).PaddingVertical(4);
            }
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().AlignLeft().Text($"Generated on {_deliveryNote.Date:dd/MM/yyyy}").FontSize(10);
            row.RelativeItem().AlignRight().Text("https://joeibbrown.art/")
                .FontSize(10)
                .FontColor(Colors.Blue.Medium);
            //.Hyperlink("https://joeibbrown.art/");  
        });
    }
}

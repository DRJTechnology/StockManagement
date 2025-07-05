using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using System.Reflection.PortableExecutable;

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
            page.Margin(20);
            page.Size(PageSizes.A5);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    void ComposeHeader(IContainer container)
    {
        var title = _deliveryNote.DirectSale ? "Invoice" : "Delivery Note";

        container.Row(row =>
        {
            row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Item().PaddingTop(5).Text(title)
                                                .FontSize(20).SemiBold().FontColor(Colors.Black);
                    if (_deliveryNote.DirectSale)
                    {
                        column.Item().Text($"Invoice Num: {_deliveryNote.Id:D5}");
                    }
                    column.Item().Text($"Date: {_deliveryNote.Date:dd/MM/yyyy}");
                });
            row.RelativeItem().AlignRight().Height(60).Image(_logoImage);
        });

    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(15).Column(column =>
        {
            column.Item().PaddingTop(25).Element(ComposeToFromSection);
            column.Item().PaddingTop(25).Element(ComposeTable);
        });
    }
    void ComposeToFromSection(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(2).Text("To:").SemiBold();
                    column.Item().Text(_deliveryNote.VenueName);
                });
                row.ConstantItem(50);
                row.RelativeItem().Column(column =>
                {
                    column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(2).Text("From:").SemiBold();
                    column.Item().Text("Joei B Brown Art");
                });
            });
        });
    }

    void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3); // Product Type  
                columns.RelativeColumn(8); // Product Name  
                columns.RelativeColumn(1); // Quantity  
                if (_deliveryNote.DirectSale)
                {
                    columns.RelativeColumn(2); // Unit Price  
                    columns.RelativeColumn(2); // Total
                }
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Type").SemiBold();
                header.Cell().Element(CellStyle).Text("Product Name").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Qty").SemiBold();
                if (_deliveryNote.DirectSale)
                {
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit").SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Total").SemiBold();
                }

                IContainer CellStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.FontSize(12).SemiBold());
            });

            foreach (var item in _deliveryNote.DetailList)
            {
                table.Cell().Element(CellStyle).Text(item.ProductTypeName);
                table.Cell().Element(CellStyle).Text(item.ProductName);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                if (_deliveryNote.DirectSale)
                {
                    table.Cell().Element(CellStyle);
                    table.Cell().Element(CellStyle);
                }

                IContainer CellStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.FontSize(11))
                                .Border(1).Padding(2);
            }
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().AlignCenter().Text("https://joeibbrown.art/")
                .FontSize(10)
                .FontColor(Colors.Blue.Medium);
        });
    }
}

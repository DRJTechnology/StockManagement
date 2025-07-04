using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using System.Net;

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
        container.Row(row =>
        {
            row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Item().PaddingTop(5).Text("Delivery Note")
                                                .FontSize(20).SemiBold().FontColor(Colors.Black);
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
                    //column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(2).Text("To:").SemiBold();
                    column.Item().Text(_deliveryNote.VenueName);
                    //column.Item().Text(Address.Street);
                    //column.Item().Text($"{Address.City}, {Address.State}");
                    //column.Item().Text(Address.Email);
                    //column.Item().Text(Address.Phone);
                });
                row.ConstantItem(50);
                row.RelativeItem().Column(column =>
                {
                    //column.Spacing(2);
                    column.Item().BorderBottom(1).PaddingBottom(2).Text("From:").SemiBold();
                    column.Item().Text("Joei B Brown Art");
                    //column.Item().Text(Address.Street);
                    //column.Item().Text($"{Address.City}, {Address.State}");
                    //column.Item().Text(Address.Email);
                    //column.Item().Text(Address.Phone);
                });
            });



        });
        //container.Column(column =>
        //{
        //    column.Spacing(2);

        //    column.Item().BorderBottom(1).PaddingBottom(5).Text("To:").SemiBold();

        //    column.Item().Text(_deliveryNote.VenueName);
        //    //column.Item().Text(Address.Street);
        //    //column.Item().Text($"{Address.City}, {Address.State}");
        //    //column.Item().Text(Address.Email);
        //    //column.Item().Text(Address.Phone);
        //});
    }

    void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3); // Product Type  
                columns.RelativeColumn(8); // Product Name  
                columns.RelativeColumn(2); // Quantity  
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Product Type").SemiBold();
                header.Cell().Element(CellStyle).Text("Product Name").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Quantity").SemiBold();

                IContainer CellStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.FontSize(12).SemiBold());
            });

            foreach (var item in _deliveryNote.DetailList)
            {
                table.Cell().Element(CellStyle).Text(item.ProductTypeName);
                table.Cell().Element(CellStyle).Text(item.ProductName);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());

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
            //row.RelativeItem().AlignLeft().Text($"Generated on {_deliveryNote.Date:dd/MM/yyyy}").FontSize(10);
            row.RelativeItem().AlignCenter().Text("https://joeibbrown.art/")
                .FontSize(10)
                .FontColor(Colors.Blue.Medium);
            //.Hyperlink("https://joeibbrown.art/");  
        });
    }
}

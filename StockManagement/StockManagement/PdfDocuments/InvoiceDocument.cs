using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StockManagement.Models;
using StockManagement.Models.Enums;

public class InvoiceDocument : IDocument
{
    private readonly StockSaleResponseModel _stockSale;
    private readonly byte[] _logoImage;
    private List<SettingResponseModel> _settings = new();

    public InvoiceDocument(StockSaleResponseModel stockSale, byte[] logoImage, List<SettingResponseModel> settings)
    {
        _stockSale = stockSale;
        _logoImage = logoImage;
        _settings = settings;
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
        var title = "TODO Invoice or Delivery Note"; // _stockSale.DirectSale ? "Invoice" : "Delivery Note";

        container.Row(row =>
        {
            row.RelativeItem().AlignLeft().Column(column =>
                {
                    column.Item().PaddingTop(5).Text(title)
                                                .FontSize(20).SemiBold().FontColor(Colors.Black);
                    //if (_stockSale.DirectSale)
                    //{
                        column.Item().Text($"Invoice Num: {_stockSale.Id:D5}");
                    //}
                    column.Item().Text($"Date: {_stockSale.Date:dd/MM/yyyy}");
                });
            row.RelativeItem().AlignRight().Height(60).Image(_logoImage);
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
                    column.Item().Text(_stockSale.LocationName);
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
                //if (_stockSale.DirectSale)
                //{
                    columns.RelativeColumn(2); // Unit Price  
                    columns.RelativeColumn(2); // Total
                //}
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Type").SemiBold();
                header.Cell().Element(CellStyle).Text("Product Name").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Qty").SemiBold();
                //if (_stockSale.DirectSale)
                //{
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit").SemiBold();
                    header.Cell().Element(CellStyle).AlignRight().Text("Total").SemiBold();
                //}

                IContainer CellStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.FontSize(12).SemiBold());
            });

            var totalQuantity = 0;
            foreach (var item in _stockSale.DetailList)
            {
                totalQuantity += item.Quantity;
                table.Cell().Element(CellStyle).Text(item.ProductTypeName);
                table.Cell().Element(CellStyle).Text(item.ProductName);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                //if (_stockSale.DirectSale)
                //{
                    table.Cell().Element(CellStyle);
                    table.Cell().Element(CellStyle);
                //}
            }

            IContainer CellStyle(IContainer container) =>
                container.DefaultTextStyle(x => x.FontSize(11))
                            .Border(1).Padding(2);

            // Add the totals column
            table.Cell().ColumnSpan(2).Element(TotalCellStyle).Text("Total");
            table.Cell().Element(TotalCellStyle).AlignRight().Text(totalQuantity.ToString());
            //if (_stockSale.DirectSale)
            //{
                table.Cell().Element(TotalCellStyle);
                table.Cell().Element(TotalCellStyle);
            //}

            IContainer TotalCellStyle(IContainer container) =>
                container.DefaultTextStyle(x => x.FontSize(11).Bold())
                            .Border(1).Padding(2);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        //if (_stockSale.DirectSale)
        //{
        //    container.Row(row =>
        //    {
        //        row.RelativeItem().Column(column =>
        //        {
        //            column.Item().Text("Payment information:").FontSize(10);
        //            column.Item().Text($"Account Name: {GetSetting(SettingEnum.BankAccountName)}").FontSize(10);
        //            column.Item().Text($"Account Number: {GetSetting(SettingEnum.BankAccountNumber)}").FontSize(10);
        //            column.Item().Text($"Account Sort Code: {GetSetting(SettingEnum.BankAccountSortCode)}").FontSize(10);
        //        });
        //        row.RelativeItem().AlignBottom().Column(column =>
        //        {
        //            column.Item().AlignRight().AlignBottom().Text(GetSetting(SettingEnum.BusinessWebsite))
        //                .FontSize(10)
        //                .FontColor(Colors.Blue.Medium);
        //        });
        //    });
        //}
        //else
        //{
            container.Row(row =>
            {
                row.RelativeItem().AlignCenter().Text(GetSetting(SettingEnum.BusinessWebsite))
                    .FontSize(10)
                    .FontColor(Colors.Blue.Medium);
            });
        //}
    }

    private string GetSetting(SettingEnum settingEnum)
    {
        var setting = _settings.FirstOrDefault(s => s.Id == (int)settingEnum);
        return setting?.SettingValue ?? string.Empty;
    }
}

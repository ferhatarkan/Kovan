using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Kovan.Infrastructure.Services;

public class PdfGenerator : IPdfGenerator
{
    public byte[] GenerateProductLabelPdf(Product product)
    {
        // QuestPDF lisansını ayarlayın (eğer gerekiyorsa)
        // QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                // Etiket boyutları (örneğin 7cm x 4cm)
                page.Size(7, 4, Unit.Centimetre);
                page.Margin(2, Unit.Millimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10)); // .FontFamily(Fonts.Helvetica) kaldırıldı, varsayılan font kullanılacak.

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(2);
                        column.Item().Text(product.Name).SemiBold().FontSize(12);
                        column.Item().Text($"{product.Price:C}").Bold().FontSize(14);
                        // QuestPDF 2023.12.1 does not provide a built-in Barcode component.
                        // Keep the SKU visible on the label until a dedicated barcode renderer is added.
                        column.Item().AlignCenter().Text(product.Sku).FontSize(10);
                    });
            });
        }).GeneratePdf();
    }

    public byte[] GenerateInvoicePdf(Kovan.Application.Features.Invoices.Dtos.InvoiceDto invoice, string? logoPath)
    {
        // Bu, InvoiceDto'nun içeriğine göre gerçek bir fatura PDF'i oluşturma mantığıdır.
        // QuestPDF dokümantasyonuna göre daha detaylı bir tasarım yapılabilir.
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10)); // .FontFamily(Fonts.Helvetica) kaldırıldı, varsayılan font kullanılacak.

                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text($"Fatura No: {invoice.InvoiceNumber}").FontSize(16).Bold();
                            column.Item().Text($"Tarih: {invoice.IssueDate:dd.MM.yyyy}");
                            column.Item().Text($"Vade: {invoice.DueDate:dd.MM.yyyy}");
                        });

                        if (!string.IsNullOrEmpty(logoPath))
                        {
                            // Logo yolu genellikle bir dosya sistemi yolu veya URL olabilir.
                            // QuestPDF'in Image() metodu byte[] veya Stream bekler.
                            // Basitlik adına burada bir placeholder bırakıyorum.
                            // row.ConstantItem(100).Image(logoPath); // Eğer logoPath doğrudan okunabilir bir yol ise
                        }
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(5);

                        column.Item().Text($"Müşteri: {invoice.CustomerName}").FontSize(12).SemiBold();
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Ürün").Bold();
                                header.Cell().Text("Miktar").Bold();
                                header.Cell().Text("Birim Fiyat").Bold();
                                header.Cell().Text("Toplam").Bold();
                            });

                            foreach (var line in invoice.InvoiceLines)
                            {
                                table.Cell().Text(line.ProductName);
                                table.Cell().Text(line.Quantity.ToString());
                                table.Cell().Text($"{line.UnitPrice:C}");
                                table.Cell().Text($"{line.Total:C}");
                            }
                        });

                        column.Item().AlignRight().Text($"Genel Toplam: {invoice.TotalAmount:C}").FontSize(14).Bold();
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Sayfa ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        }).GeneratePdf();
    }
}

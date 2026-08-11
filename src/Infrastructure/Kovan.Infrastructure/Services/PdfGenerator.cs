using Kovan.Application.Common.Models;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Features.Invoices.Queries;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Kovan.Infrastructure.Services;

public class PdfGenerator : IPdfGenerator
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly PdfSettings _pdfSettings;

    public PdfGenerator(IWebHostEnvironment webHostEnvironment, IOptions<PdfSettings> pdfSettings)
    {
        _webHostEnvironment = webHostEnvironment;
        _pdfSettings = pdfSettings.Value;
    }

    public PdfFileDto GenerateInvoicePdf(InvoiceDto invoice, string? tenantLogoPath)
    {
        // wwwroot klasöründeki logo dosyasının tam yolunu al
        var logoPath = Path.Combine(_webHostEnvironment.WebRootPath, tenantLogoPath ?? _pdfSettings.LogoPath);
        byte[]? logoData = null;

        // Logo dosyası varsa, içeriğini oku
        if (File.Exists(logoPath))
        {
            logoData = File.ReadAllBytes(logoPath);
        }

        // Logo verisini PDF dökümanına gönder
        var document = new InvoiceDocument(invoice, logoData);
        var pdfBytes = document.GeneratePdf();

        return new PdfFileDto
        {
            Content = pdfBytes,
            FileName = $"Fatura-{invoice.InvoiceNumber}.pdf"
        };
    }
}

public class InvoiceDocument : IDocument
{
    private readonly InvoiceDto _invoice;
    private readonly byte[]? _logoData;

    public InvoiceDocument(InvoiceDto invoice, byte[]? logoData)
    {
        _invoice = invoice;
        _logoData = logoData;
    }

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            if (_logoData != null)
            {
                row.ConstantItem(140).Image(_logoData);
            }
            else
            {
                row.ConstantItem(140).Height(60).Placeholder("Şirket Logosu");
            }

            row.RelativeItem().Column(column =>
            {
                column.Item().AlignRight().Text("FATURA").SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);
                column.Item().AlignRight().Text($"#{_invoice.InvoiceNumber}").Bold();
                column.Item().AlignRight().Text($"Düzenlenme Tarihi: {_invoice.IssueDate:d}");
                column.Item().AlignRight().Text($"Vade Tarihi: {_invoice.DueDate:d}");
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(30).Column(column =>
        {
            column.Spacing(30);

            // Satıcı ve Alıcı Bilgileri
            column.Item().Row(row =>
            {
                // Satıcı (Sizin Şirketiniz)
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Satıcı Firma").SemiBold().Underline();
                    col.Item().Text("Kovan A.Ş.");
                    col.Item().Text("Teknoloji Cd. No:1");
                    col.Item().Text("Teknopark, İstanbul");
                    col.Item().Text("info@kovan.com");
                });

                // Alıcı (Müşteri)
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Alıcı Firma").SemiBold().Underline();
                    col.Item().Text(_invoice.CustomerName);
                    // Müşterinin adresi, vergi numarası gibi bilgiler CustomerDto'ya eklenip burada gösterilebilir.
                });
            });

            column.Item().Element(ComposeTable);

            // Toplamlar ve Banka Bilgileri
            column.Item().Row(row =>
            {
                // Banka Bilgileri
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Ödeme Bilgileri").SemiBold().Underline();
                    col.Item().Text("Banka Adı: Kovan Bank");
                    col.Item().Text("IBAN: TR00 0000 0000 0000 0000 0000");
                    col.Item().Text("Açıklama kısmına fatura numarasını belirtiniz.");
                });

                // Toplamlar
                row.RelativeItem().AlignRight().Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Ara Toplam:");
                        r.ConstantItem(100).AlignRight().Text($"{_invoice.TotalAmount:C}");
                    });
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Toplam KDV:");
                        r.ConstantItem(100).AlignRight().Text($"{_invoice.TotalVatAmount:C}");
                    });
                    col.Item().PaddingTop(5).BorderTop(1).Row(r =>
                    {
                        r.RelativeItem().Text("Genel Toplam:").SemiBold();
                        r.ConstantItem(100).AlignRight().Text($"{_invoice.GrandTotal:C}").SemiBold();
                    });
                });
            });
        });
    }

    void ComposeTable(IContainer container)
    {
        var headerStyle = TextStyle.Default.SemiBold();

        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(4); // Ürün Adı
                columns.RelativeColumn(1); // Miktar
                columns.RelativeColumn(2); // Birim Fiyat
                columns.RelativeColumn(1); // KDV %
                columns.RelativeColumn(2); // Ara Toplam
                columns.RelativeColumn(2); // Toplam
            });

            table.Header(header =>
            {
                header.Cell().Text("Ürün / Hizmet").Style(headerStyle);
                header.Cell().AlignRight().Text("Miktar").Style(headerStyle);
                header.Cell().AlignRight().Text("Birim Fiyat").Style(headerStyle);
                header.Cell().AlignRight().Text("KDV").Style(headerStyle);
                header.Cell().AlignRight().Text("Net Tutar").Style(headerStyle);
                header.Cell().AlignRight().Text("Toplam Tutar").Style(headerStyle);
            });

            foreach (var item in _invoice.InvoiceLines)
            {
                table.Cell().Text(item.ProductName);
                table.Cell().AlignRight().Text(item.Quantity.ToString());
                table.Cell().AlignRight().Text($"{item.UnitPrice:C}");
                table.Cell().AlignRight().Text($"%{item.VatRate}");
                table.Cell().AlignRight().Text($"{item.NetTotal:C}");
                table.Cell().AlignRight().Text($"{item.GrossTotal:C}");
            }
        });
    }
}
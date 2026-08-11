using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Kovan.Application.Features.Products.Queries.GetProductLabel;

public class GetProductLabelQueryHandler : IRequestHandler<GetProductLabelQuery, ProductLabelDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductLabelQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductLabelDto> Handle(GetProductLabelQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Etiket boyutları (örneğin 7cm x 4cm)
                page.Size(7, 4, Unit.Centimetre);
                page.Margin(2, Unit.Millimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Helvetica));

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(2);

                        // Ürün Adı
                        column.Item().Text(product.Name).SemiBold().FontSize(12);

                        // Fiyat
                        column.Item().Text($"{product.Price:C}").Bold().FontSize(14);

                        // Barkod (SKU'dan oluşturuluyor)
                        column.Item().AlignCenter().Barcode(barcode =>
                        {
                            barcode.Type(BarcodeType.Code128);
                            barcode.Value(product.Sku);
                            barcode.Height(1, Unit.Centimetre);
                        });
                    });
            });
        }).GeneratePdf();

        return new ProductLabelDto
        {
            FileContents = pdfBytes,
            FileName = $"label-{product.Sku}.pdf"
        };
    }
}
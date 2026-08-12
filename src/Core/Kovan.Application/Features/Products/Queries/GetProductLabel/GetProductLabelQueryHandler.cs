using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Products.Queries.GetProductLabel;

public class GetProductLabelQueryHandler : IRequestHandler<GetProductLabelQuery, ProductLabelDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPdfGenerator _pdfGenerator;

    public GetProductLabelQueryHandler(IApplicationDbContext context, IPdfGenerator pdfGenerator)
    {
        _context = context;
        _pdfGenerator = pdfGenerator;
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

        var pdfBytes = _pdfGenerator.GenerateProductLabelPdf(product);

        return new ProductLabelDto
        {
            FileContents = pdfBytes,
            FileName = $"label-{product.Sku}.pdf"
        };
    }
}
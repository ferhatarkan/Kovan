using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using Kovan.Domain.Entities;
using MediatR;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoicePdf;

public class GetInvoicePdfQueryHandler : IRequestHandler<GetInvoicePdfQuery, PdfFileDto>
{
    private readonly ISender _sender;
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IApplicationDbContext _context;

    public GetInvoicePdfQueryHandler(ISender sender, IPdfGenerator pdfGenerator, IApplicationDbContext context)
    {
        _sender = sender;
        _pdfGenerator = pdfGenerator;
        _context = context;
    }

    public async Task<PdfFileDto> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken)
    {
        // 1. Fatura verilerini almak için mevcut sorguyu yeniden kullan.
        var invoiceDto = await _sender.Send(new GetInvoiceByIdQuery { Id = request.InvoiceId }, cancellationToken);

        // 2. Faturanın ait olduğu kiracıyı bul.
        // Not: Bu sorgu, multi-tenancy filtresi sayesinde sadece mevcut kiracının faturasını bulacaktır.
        var invoice = await _context.Invoices.FindAsync(new object[] { request.InvoiceId }, cancellationToken)
                      ?? throw new NotFoundException(nameof(Invoice), request.InvoiceId);

        var tenant = await _context.Tenants.FindAsync(new object[] { invoice.TenantId }, cancellationToken);

        // 3. Kiracının logo yolunu al ve PDF oluşturma servisini çağır.
        var pdfFile = _pdfGenerator.GenerateInvoicePdf(invoiceDto, tenant?.LogoPath);

        return pdfFile;
    }
}
using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoicePdf;

public class GetInvoicePdfQuery : IRequest<PdfFileDto>
{
    public Guid InvoiceId { get; set; }
}
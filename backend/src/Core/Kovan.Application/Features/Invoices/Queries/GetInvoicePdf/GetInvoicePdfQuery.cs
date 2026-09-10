using MediatR;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoicePdf;

public class GetInvoicePdfQuery : IRequest<GetInvoicePdfResult>
{
    public Guid InvoiceId { get; set; }
}
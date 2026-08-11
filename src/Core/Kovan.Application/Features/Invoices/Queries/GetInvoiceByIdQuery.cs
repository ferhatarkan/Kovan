using MediatR;

namespace Kovan.Application.Features.Invoices.Queries;

public sealed class GetInvoiceByIdQuery : IRequest<InvoiceDto>
{
    public Guid Id { get; init; }
}

using Kovan.Application.Features.Invoices.Queries.GetInvoiceById;
using MediatR;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoiceById;

public sealed class GetInvoiceByIdQuery : IRequest<GetInvoiceByIdResult>
{
    public Guid Id { get; init; }
}

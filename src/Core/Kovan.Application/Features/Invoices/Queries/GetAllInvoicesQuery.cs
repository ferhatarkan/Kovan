using MediatR;

namespace Kovan.Application.Features.Invoices.Queries;

public sealed class GetAllInvoicesQuery : IRequest<List<InvoiceDto>>
{
}

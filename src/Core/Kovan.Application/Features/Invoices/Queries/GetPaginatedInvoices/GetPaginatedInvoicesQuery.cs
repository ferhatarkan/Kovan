using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Invoices.Queries.GetPaginatedInvoices;

public class GetPaginatedInvoicesQuery : IRequest<PaginatedList<InvoiceDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    // İsteğe bağlı olarak filtreleme veya sıralama parametreleri eklenebilir.
}

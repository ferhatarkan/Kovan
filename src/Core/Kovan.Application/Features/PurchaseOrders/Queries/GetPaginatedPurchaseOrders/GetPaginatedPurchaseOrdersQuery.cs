using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;

public class GetPaginatedPurchaseOrdersQuery : IRequest<PaginatedList<GetPaginatedPurchaseOrdersResult>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
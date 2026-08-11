using Kovan.Application.Common.Models;
using MediatR;
using Kovan.Application.Features.PurchaseOrders.Dtos; // Merkezi DTO'nun namespace'ini ekle

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;

public class GetPaginatedPurchaseOrdersQuery : IRequest<PaginatedList<PurchaseOrderDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
}
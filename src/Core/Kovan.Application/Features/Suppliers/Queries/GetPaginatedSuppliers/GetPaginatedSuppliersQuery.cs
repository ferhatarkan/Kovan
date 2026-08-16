using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Suppliers.Queries.GetPaginatedSuppliers;

public class GetPaginatedSuppliersQuery : IRequest<PaginatedList<GetPaginatedSuppliersResult>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
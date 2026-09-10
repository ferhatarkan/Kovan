using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Customers.Queries.GetPaginatedCustomers;

public class GetPaginatedCustomersQuery : IRequest<PaginatedList<GetPaginatedCustomersResult>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Customers.Queries.GetPaginatedCustomers;

public class GetPaginatedCustomersQuery : IRequest<PaginatedList<CustomerDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

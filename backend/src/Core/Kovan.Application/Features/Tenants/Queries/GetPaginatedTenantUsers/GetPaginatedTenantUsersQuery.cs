using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Tenants.Queries.GetPaginatedTenantUsers;

public class GetPaginatedTenantUsersQuery : IRequest<PaginatedList<GetPaginatedTenantUsersResult>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
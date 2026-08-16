using AutoMapper;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Mappings;
using Kovan.Application.Common.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Tenants.Queries.GetPaginatedTenantUsers;

public class GetPaginatedTenantUsersQueryHandler : IRequestHandler<GetPaginatedTenantUsersQuery, PaginatedList<GetPaginatedTenantUsersResult>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public GetPaginatedTenantUsersQueryHandler(ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<PaginatedList<GetPaginatedTenantUsersResult>> Handle(GetPaginatedTenantUsersQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.TenantId))
        {
            return new PaginatedList<GetPaginatedTenantUsersResult>(new List<GetPaginatedTenantUsersResult>(), 0, request.PageNumber, request.PageSize);
        }

        return await _identityService.GetPaginatedUsersAsync(_currentUserService.TenantId, request.PageNumber, request.PageSize, cancellationToken);
    }
}
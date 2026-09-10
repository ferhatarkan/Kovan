using Kovan.Application.Common.Models;
using Kovan.Application.Features.Tenants.Queries.GetPaginatedTenantUsers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<PaginatedList<GetPaginatedTenantUsersResult>> GetPaginatedUsersAsync(string tenantId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Dictionary<string, string>> GetUserNamesAsync(IEnumerable<string> userIds, CancellationToken cancellationToken);
}
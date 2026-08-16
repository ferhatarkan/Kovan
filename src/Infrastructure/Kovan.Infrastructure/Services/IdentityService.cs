using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Mappings;
using Kovan.Application.Common.Models;
using Kovan.Application.Features.Tenants.Queries.GetPaginatedTenantUsers;
using Kovan.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<PaginatedList<GetPaginatedTenantUsersResult>> GetPaginatedUsersAsync(string tenantId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(tenantId, out var tenantGuid))
        {
            return new PaginatedList<GetPaginatedTenantUsersResult>(new List<GetPaginatedTenantUsersResult>(), 0, pageNumber, pageSize);
        }

        var query = _userManager.Users
            .Where(u => u.TenantId == tenantGuid)
            .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
            .Select(u => new GetPaginatedTenantUsersResult
            {
                UserId = u.Id,
                FullName = $"{u.FirstName} {u.LastName}".Trim(),
                Email = u.Email ?? string.Empty
                // Rol bilgisi için ek bir sorgu gerekebilir.
            });

        return await query.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
    }

    public async Task<Dictionary<string, string>> GetUserNamesAsync(IEnumerable<string> userIds, CancellationToken cancellationToken)
    {
        if (userIds == null || !userIds.Any())
        {
            return new Dictionary<string, string>();
        }

        return await _userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);
    }
}

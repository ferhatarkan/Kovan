using System.Security.Claims;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // JWT token içerisindeki 'nameid' claim'inden kullanıcı ID'sini alır.
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    // JWT token içerisindeki özel 'tenant_id' claim'inden kiracı ID'sini alır.
    public string? TenantId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("tenant_id");
}
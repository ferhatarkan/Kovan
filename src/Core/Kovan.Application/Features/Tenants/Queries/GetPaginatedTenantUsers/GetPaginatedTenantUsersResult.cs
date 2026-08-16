namespace Kovan.Application.Features.Tenants.Queries.GetPaginatedTenantUsers;

public class GetPaginatedTenantUsersResult
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Kullanıcının rolü
}
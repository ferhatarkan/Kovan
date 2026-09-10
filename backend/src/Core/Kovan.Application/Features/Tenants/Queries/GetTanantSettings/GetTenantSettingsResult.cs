namespace Kovan.Application.Features.Tenants.Queries.GetTenantSettings;

public class GetTenantSettingsResult
{
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    // Diğer kiracı ayarları buraya eklenebilir.
}
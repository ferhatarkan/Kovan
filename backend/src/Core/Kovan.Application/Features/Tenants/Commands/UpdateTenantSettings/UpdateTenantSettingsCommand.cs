using MediatR;

namespace Kovan.Application.Features.Tenants.Commands.UpdateTenantSettings;

public class UpdateTenantSettingsCommand : IRequest
{
    public string? LogoPath { get; set; }
}
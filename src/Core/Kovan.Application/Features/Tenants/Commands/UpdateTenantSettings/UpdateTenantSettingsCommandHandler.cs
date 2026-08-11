using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;

namespace Kovan.Application.Features.Tenants.Commands.UpdateTenantSettings;

public class UpdateTenantSettingsCommandHandler : IRequestHandler<UpdateTenantSettingsCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTenantSettingsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateTenantSettingsCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUserService.TenantId, out var tenantId))
        {
            throw new UnauthorizedAccessException("Geçerli bir kiracı kimliği bulunamadı.");
        }

        var tenant = await _context.Tenants.FindAsync(new object[] { tenantId }, cancellationToken)
                     ?? throw new NotFoundException(nameof(Tenant), tenantId);

        tenant.SetLogoPath(request.LogoPath);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
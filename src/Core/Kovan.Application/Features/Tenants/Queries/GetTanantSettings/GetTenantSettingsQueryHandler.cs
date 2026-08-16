using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using FluentValidation.Results;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Tenants.Queries.GetTenantSettings;

public class GetTenantSettingsQueryHandler : IRequestHandler<GetTenantSettingsQuery, GetTenantSettingsResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetTenantSettingsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<GetTenantSettingsResult> Handle(GetTenantSettingsQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUserService.TenantId, out var tenantId))
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(nameof(_currentUserService.TenantId), "Geçerli bir kiracı ID'si bulunamadı.") };
            throw new ValidationException(failures);
        }

        var tenant = await _context.Tenants.Where(t => t.Id == tenantId)
            .ProjectTo<GetTenantSettingsResult>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException(nameof(Tenant), tenantId);

        return tenant;
    }
}
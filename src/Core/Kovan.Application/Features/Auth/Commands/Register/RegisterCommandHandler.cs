using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Features.Auth.Commands.Login;
using Kovan.Domain.Constants;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly ISender _sender; // LoginCommand'ı tetiklemek için

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context, ISender sender)
    {
        _userManager = userManager;
        _context = context;
        _sender = sender;
    }

    public async Task<LoginResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            // TenantId'yi burada atayacağız.
        };

        // Bu, yeni bir kiracı ve ilk kullanıcısını oluşturan bir senaryodur.
        // 1. Yeni bir Tenant oluştur.
        var newTenant = Tenant.Create($"{request.FirstName}'s Company");
        _context.Tenants.Add(newTenant);

        // 2. Kullanıcının TenantId'sini ata.
        user.TenantId = newTenant.Id;

        // 3. Kullanıcıyı oluştur.
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            // Identity hatalarını ValidationException'a dönüştürerek fırlatıyoruz.
            var validationFailures = result.Errors
                .Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description));
            throw new ValidationException(validationFailures);
        }

        // Yeni kiracının ilk kullanıcısına "Admin" rolünü ata.
        await _userManager.AddToRoleAsync(user, Roles.Admin);

        // Kayıt başarılı olduktan sonra, kullanıcıyı otomatik olarak login yap.
        var loginCommand = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        return await _sender.Send(loginCommand, cancellationToken);
    }
}

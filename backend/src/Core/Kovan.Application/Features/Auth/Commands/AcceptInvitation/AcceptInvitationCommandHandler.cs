using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Features.Auth.Commands.Login;
using Kovan.Domain.Constants;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Auth.Commands.AcceptInvitation;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, LoginResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISender _sender;

    public AcceptInvitationCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager, ISender sender)
    {
        _context = context;
        _userManager = userManager;
        _sender = sender;
    }

    public async Task<LoginResponseDto> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        // 1. Daveti bul. DbContext'teki global tenant filtresini geçici olarak devre dışı bırakarak arama yap.
        var invitation = await _context.UserInvitations
            .IgnoreQueryFilters() // Önemli: Henüz bir kiracıya ait olmayan bir kullanıcı arama yapıyor.
            .FirstOrDefaultAsync(i => i.InvitationToken == request.Token, cancellationToken);

        if (invitation == null)
        {
            throw new NotFoundException(nameof(UserInvitation), request.Token);
        }

        // 2. Daveti kabul et (süre kontrolü ve zaten kabul edilip edilmediği kontrolü domain entity'si içinde yapılır).
        invitation.Accept();

        // 3. Yeni kullanıcıyı oluştur.
        var user = new ApplicationUser
        {
            TenantId = invitation.TenantId,
            Email = invitation.Email,
            UserName = invitation.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
        }

        // 4. Yeni kullanıcıya varsayılan "User" rolünü ata.
        await _userManager.AddToRoleAsync(user, Roles.User);

        await _context.SaveChangesAsync(cancellationToken);

        // 5. Kullanıcıyı otomatik olarak login yap.
        var loginCommand = new LoginCommand { Email = user.Email, Password = request.Password };
        return await _sender.Send(loginCommand, cancellationToken);
    }
}
using System.Security.Cryptography;
using System.Web;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Tenants.Commands.InviteUser;

public class InviteUserCommandHandler : IRequestHandler<InviteUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public InviteUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_currentUserService.TenantId, out var tenantId) || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            throw new UnauthorizedAccessException("Geçerli bir kiracı veya kullanıcı kimliği bulunamadı.");
        }

        // Bu e-posta ile zaten bir kullanıcı var mı?
        var userExists = await _userManager.FindByEmailAsync(request.Email);
        if (userExists != null && userExists.TenantId == tenantId)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Email", "Bu e-posta adresine sahip bir kullanıcı zaten mevcut.") });
        }

        // Bu e-posta için zaten geçerli bir davet var mı?
        var existingInvitation = await _context.UserInvitations
            .FirstOrDefaultAsync(i => i.Email == request.Email && i.ExpiresAt > DateTime.UtcNow && !i.IsAccepted, cancellationToken);

        if (existingInvitation != null)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Email", "Bu e-posta adresi için zaten geçerli bir davet bulunmaktadır.") });
        }

        // Yeni davet oluştur
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var invitation = UserInvitation.Create(tenantId, request.Email, token, TimeSpan.FromDays(7), _currentUserService.UserId);

        _context.UserInvitations.Add(invitation);
        await _context.SaveChangesAsync(cancellationToken);

        // Davet e-postası gönder
        var encodedToken = HttpUtility.UrlEncode(token);
        var registrationLink = $"https://your-frontend-app.com/accept-invitation?token={encodedToken}";

        await _emailService.SendEmailAsync(request.Email, "Kovan Platformuna Davet", $"Kovan platformuna davet edildiniz. Kayıt olmak için lütfen şu bağlantıya tıklayın: <a href='{registrationLink}'>Kaydı Tamamla</a>");
    }
}
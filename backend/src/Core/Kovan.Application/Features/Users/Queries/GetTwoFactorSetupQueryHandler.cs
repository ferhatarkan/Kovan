using System.Text.Encodings.Web;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Users.Queries;

public class GetTwoFactorSetupQueryHandler : IRequestHandler<GetTwoFactorSetupQuery, GetTwoFactorSetupDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public GetTwoFactorSetupQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<GetTwoFactorSetupDto> Handle(GetTwoFactorSetupQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        // Kullanıcının mevcut 2FA anahtarını temizle ve yenisini oluştur.
        await _userManager.ResetAuthenticatorKeyAsync(user);
        var sharedKey = await _userManager.GetAuthenticatorKeyAsync(user);

        if (string.IsNullOrEmpty(sharedKey))
        {
            throw new InvalidOperationException("Authenticator anahtarı oluşturulamadı.");
        }

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new InvalidOperationException("Kullanıcının e-posta adresi bulunamadığı için 2FA URI oluşturulamadı.");
        }

        var authenticatorUri = $"otpauth://totp/{UrlEncoder.Default.Encode("KovanApp")}:{UrlEncoder.Default.Encode(user.Email)}?secret={sharedKey}&issuer={UrlEncoder.Default.Encode("KovanApp")}&digits=6";

        return new GetTwoFactorSetupDto
        {
            SharedKey = sharedKey,
            AuthenticatorUri = authenticatorUri
        };
    }
}
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Users.Commands.EnableTwoFactor;

public class EnableTwoFactorCommandHandler : IRequestHandler<EnableTwoFactorCommand, IEnumerable<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public EnableTwoFactorCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<string>> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        // Girilen kodun geçerliliğini kontrol et.
        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, request.VerificationCode);

        if (!isValid)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("VerificationCode", "Doğrulama kodu geçersiz.") });
        }

        // 2FA'yı etkinleştir.
        await _userManager.SetTwoFactorEnabledAsync(user, true);

        // Kurtarma kodları oluştur ve döndür.
        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        return recoveryCodes ?? Enumerable.Empty<string>();
    }
}
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Features.Auth.Commands.Login;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity; // Bu satır zaten vardı, ancak tekrar kontrol etmekte fayda var.

namespace Kovan.Application.Features.Auth.Commands.LoginWith2fa;

public class LoginWith2faCommandHandler : IRequestHandler<LoginWith2faCommand, LoginResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public LoginWith2faCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginWith2faCommand request, CancellationToken cancellationToken)
    {
        // SignInManager, 2FA için oturum açmış bir kullanıcı bekler. Önce kullanıcıyı almalıyız.
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null || user.Email != request.Email)
        {
            throw new NotFoundException(nameof(ApplicationUser), request.Email);
        }

        // 2FA kodunu doğrula
        var result = await _signInManager.TwoFactorSignInAsync("Email", request.TwoFactorCode, false, false);

        if (!result.Succeeded)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("TwoFactorCode", "2FA kodu geçersiz.") });
        }

        // 2FA doğrulaması başarılıysa, token oluştur.
        var userRoles = await _userManager.GetRolesAsync(user);
        var tokenString = await _tokenService.GenerateJwtTokenAsync(user, userRoles);

        // Refresh token oluştur ve kaydet
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto { Token = tokenString, RefreshToken = refreshToken, Is2faRequired = false };
    }
}
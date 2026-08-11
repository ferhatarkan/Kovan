using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            var failure = new FluentValidation.Results.ValidationFailure("Login", "Kullanıcı adı veya şifre hatalı.");
            throw new ValidationException(new[] { failure });
        }

        // Kullanıcı için 2FA etkin mi diye kontrol et.
        if (user.TwoFactorEnabled)
        {
            return new LoginResponseDto
            {
                Is2faRequired = true,
                Message = "İki faktörlü kimlik doğrulama kodu gereklidir."
            };
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var token = await _tokenService.GenerateJwtTokenAsync(user, userRoles);

        // Refresh token oluştur ve kaydet
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Refresh token geçerlilik süresi
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto { Token = token, RefreshToken = refreshToken, Is2faRequired = false };
    }
}

using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Features.Auth.Commands.Login;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Kovan.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal?.Identity?.Name is null)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Token", "Geçersiz erişim token'ı.") });
        }

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Token", "Geçersiz yenileme token'ı.") });
        }

        // Yeni token'ları oluştur
        var userRoles = await _userManager.GetRolesAsync(user);
        var newAccessToken = await _tokenService.GenerateJwtTokenAsync(user, userRoles);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Yeni refresh token'ı kullanıcıya kaydet
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}
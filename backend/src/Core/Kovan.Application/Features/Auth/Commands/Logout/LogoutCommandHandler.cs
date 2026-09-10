using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public LogoutCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        // Refresh token'ı ve son kullanma tarihini temizle
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.MinValue; // Veya DateTime.UtcNow.AddDays(-1)

        await _userManager.UpdateAsync(user);
    }
}
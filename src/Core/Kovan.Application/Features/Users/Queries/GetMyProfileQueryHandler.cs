using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Users.Queries;

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, MyProfileDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public GetMyProfileQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<MyProfileDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        return new MyProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            ProfilePicturePath = user.ProfilePicturePath
        };
    }
}
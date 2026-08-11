using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Users.Commands.UpdateMyProfile;

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public UpdateMyProfileCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
        }
    }
}
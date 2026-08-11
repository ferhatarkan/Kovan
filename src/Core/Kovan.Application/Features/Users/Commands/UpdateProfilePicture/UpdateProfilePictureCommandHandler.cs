using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Users.Commands.UpdateProfilePicture;

public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileStorageService _fileStorageService;

    public UpdateProfilePictureCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IFileStorageService fileStorageService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _fileStorageService = fileStorageService;
    }

    public async Task<string> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        var user = await _userManager.FindByIdAsync(userId)
                   ?? throw new NotFoundException(nameof(ApplicationUser), userId);

        // Dosyayı kaydet ve göreli yolunu al
        var filePath = await _fileStorageService.SaveFileAsync(request.FileContent, request.FileName, "images/avatars");

        // Kullanıcının profil resmi yolunu güncelle
        user.ProfilePicturePath = filePath;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            // TODO: Dosya kaydedildi ama DB güncellenemediyse, kaydedilen dosyayı silmek için bir mekanizma eklenebilir.
            throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
        }

        return filePath;
    }
}
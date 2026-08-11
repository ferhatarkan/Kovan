using Kovan.Application.Common.Exceptions;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kovan.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            // Güvenlik nedeniyle kullanıcının var olup olmadığını belli etme.
            // Sadece genel bir hata mesajı döndür.
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Token", "Geçersiz e-posta veya şifre sıfırlama anahtarı.") });
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
        }
    }
}
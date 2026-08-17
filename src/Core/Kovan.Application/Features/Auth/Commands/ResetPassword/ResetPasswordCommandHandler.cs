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
            // Güvenlik Notu: Kullanıcı bulunamazsa, bir e-posta adresinin sistemde kayıtlı olup olmadığını
            // belli etmemek için hata fırlatılmaz. İşlem sessizce sonlandırılır.
            // Bu, kullanıcı enumerasyon saldırılarını (user enumeration attacks) önler.
            // İstemci tarafına her zaman işlemin başarılı olduğuna dair bir mesaj gösterilmelidir.
            return;
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure("Token", e.Description)));
        }
    }
}
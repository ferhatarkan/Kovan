using FluentValidation;

namespace Kovan.Application.Features.Tenants.Commands.InviteUser;

public class InviteUserCommandValidator : AbstractValidator<InviteUserCommand>
{
    public InviteUserCommandValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta adresi girilmelidir.");
        RuleFor(v => v.Role).NotEmpty().WithMessage("Kullanıcı rolü belirtilmelidir.");
    }
}
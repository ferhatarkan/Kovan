using FluentValidation;

namespace Kovan.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("İsim alanı zorunludur.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Soyisim alanı zorunludur.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(v => v.Password).NotEmpty().WithMessage("Şifre boş olamaz.");
    }
}
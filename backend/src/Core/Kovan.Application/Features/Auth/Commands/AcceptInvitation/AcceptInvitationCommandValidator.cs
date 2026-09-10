using FluentValidation;

namespace Kovan.Application.Features.Auth.Commands.AcceptInvitation;

public class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
{
    public AcceptInvitationCommandValidator()
    {
        RuleFor(v => v.Token)
            .NotEmpty().WithMessage("Davet token'ı boş olamaz.");

        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("İsim alanı zorunludur.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Soyisim alanı zorunludur.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

        RuleFor(v => v.ConfirmPassword)
            .Equal(v => v.Password).WithMessage("Şifreler eşleşmiyor.");
    }
}
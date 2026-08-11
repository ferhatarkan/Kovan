using FluentValidation;

namespace Kovan.Application.Features.Users.Commands.ChangeMyPassword;

public class ChangeMyPasswordCommandValidator : AbstractValidator<ChangeMyPasswordCommand>
{
    public ChangeMyPasswordCommandValidator()
    {
        RuleFor(v => v.CurrentPassword)
            .NotEmpty().WithMessage("Mevcut şifre boş olamaz.");

        RuleFor(v => v.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
            .MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır.")
            .NotEqual(v => v.CurrentPassword).WithMessage("Yeni şifre, mevcut şifre ile aynı olamaz.");

        RuleFor(v => v.ConfirmNewPassword)
            .Equal(v => v.NewPassword).WithMessage("Yeni şifreler eşleşmiyor.");
    }
}
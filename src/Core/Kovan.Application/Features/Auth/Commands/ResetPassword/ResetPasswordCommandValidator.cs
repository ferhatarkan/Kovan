using FluentValidation;

namespace Kovan.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(v => v.Token).NotEmpty().WithMessage("Şifre sıfırlama anahtarı boş olamaz.");

        RuleFor(v => v.Password).NotEmpty().MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

        RuleFor(v => v.ConfirmPassword).Equal(v => v.Password).WithMessage("Şifreler eşleşmiyor.");
    }
}
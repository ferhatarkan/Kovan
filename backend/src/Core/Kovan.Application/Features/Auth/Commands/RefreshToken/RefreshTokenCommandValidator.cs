using FluentValidation;

namespace Kovan.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(v => v.AccessToken)
            .NotEmpty().WithMessage("Erişim token'ı boş olamaz.");
        RuleFor(v => v.RefreshToken)
            .NotEmpty().WithMessage("Yenileme token'ı boş olamaz.");
    }
}
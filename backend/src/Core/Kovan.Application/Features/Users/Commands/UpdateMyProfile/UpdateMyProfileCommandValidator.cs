using FluentValidation;

namespace Kovan.Application.Features.Users.Commands.UpdateMyProfile;

public class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("İsim alanı zorunludur.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Soyisim alanı zorunludur.");
    }
}